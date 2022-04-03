using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FittimePanelApi.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FittimePanelApi.Models;
using FittimePanelApi.Services;
using FittimePanelApi.INotifications;
using FittimePanelApi.IControllers;
using FittimePanelApi.Models.Notifications;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        private readonly ISmsPanel _smsPanel;

        public AuthController(UserManager<User> userManager,
                                ILogger<AuthController> logger,
                                IMapper mapper,
                                IAuthManager authManager,
                                ISmsPanel smsPanel)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
            _smsPanel = smsPanel;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<User>(userDTO);
                user.UserName = userDTO.UserName;
                user.RegistrationDate = DateTime.Now;
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                try
                {
                    await _smsPanel.SendSMS(new SendSmsDTO()
                    {
                        To = new string[] { user.PhoneNumber },
                        Text = String.Format("{0} عزیز شما با موفقیت ثبت نام کردید. فیت تایم", user.FullName)
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Can't send sms massages");
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Login Attempt for {userDTO.UserName} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authManager.ValidateUser(userDTO))
                {
                    return Unauthorized();
                }

                try
                {
                    var user = await _userManager.FindByNameAsync(userDTO.UserName);
                    user.LastLoginTime = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Can't update user for login time {nameof(Login)}");
                    throw;
                }

                return Accepted(new { Token = await _authManager.CreateToken(userDTO.RememberMe) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("roles")]
        public async Task<IActionResult> UserRoles()
        {
            throw new NotImplementedException();
        }

        private static string ConvertPersianNumberToEnglish(string input)
        {
            string EnglishNumbers = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    EnglishNumbers += char.GetNumericValue(input, i);
                }
                else
                {
                    EnglishNumbers += input[i].ToString();
                }
            }
            return EnglishNumbers;
        }

        [HttpPost]
        [Route("forgot")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            _logger.LogInformation($"Forget Password Attempt for {forgotPasswordDTO.UserName} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByNameAsync(forgotPasswordDTO.UserName);
                if (user == null)
                {
                    _logger.LogError($"Something Went Wrong in the {nameof(ForgotPassword)} User not exist");
                    return Problem($"Something Went Wrong in the {nameof(ForgotPassword)} User not exist", statusCode: 500);
                }
                if (user.PhoneNumber != forgotPasswordDTO.PhoneNumber)
                {
                    _logger.LogError($"Something Went Wrong in the {nameof(ForgotPassword)} Phone number wrong");
                    return Problem($"Something Went Wrong in the {nameof(ForgotPassword)} Phone number wrong", statusCode: 500);
                }

                var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");

                try
                {
                    await _smsPanel.SendSMS(new SendSmsDTO()
                    {
                        To = new string[] { user.PhoneNumber },
                        Text = String.Format("کد تغییر رمز عبور شما {0} می باشد. فیت تایم", token)
                    });
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, $"Can't send sms massages");
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("reset")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            _logger.LogInformation($"Reset Password Attempt for {resetPasswordDTO.UserName} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByNameAsync(resetPasswordDTO.UserName);
                if (user == null)
                {
                    _logger.LogError($"Something Went Wrong in the {nameof(ResetPassword)} User not exist");
                    return Problem($"Something Went Wrong in the {nameof(ResetPassword)} User not exist", statusCode: 500);
                }
                var tokenVerified = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", resetPasswordDTO.Token);
                if (!tokenVerified)
                    return UnprocessableEntity($"Something Went Wrong in the {nameof(ResetPassword)} Token not verified");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDTO.Password);

                if (!result.Succeeded)
                    return UnprocessableEntity("Weak password");

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ResetPassword)}");
                return Problem($"Something Went Wrong in the {nameof(ResetPassword)}", statusCode: 500);
            }
        }

        private static bool CheckCodeMelli(string nationalCode)
        {
            bool result;
            try
            {

                var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
                if (allDigitEqual.Contains(nationalCode)) return false;


                var chArray = nationalCode.ToCharArray();
                var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                var a = Convert.ToInt32(chArray[9].ToString());

                var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
                var c = b % 11;

                return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
            }
            catch { result = false; }

            return result;
        }
    }
}
