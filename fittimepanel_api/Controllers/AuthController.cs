using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FittimePanelApi.Models;
using System.Text.RegularExpressions;

namespace FittimePanelApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly AppDb _context;

        public AuthController(IJwtAuthenticationManager jwtAuthenticationManager, AppDb context)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Registration([FromBody] UserRegister user)
        {
            // validation
            if (!(user.Username.Length > 0))
                return BadRequest(new ErrorResponse
                {
                    Code = 100,
                    Message = "کد ملی درست وارد نشده است"
                });
            else
            {
                user.Username = ConvertPersianNumberToEnglish(user.Username);
                if(!CheckCodeMelli(user.Username))
                    return BadRequest(new ErrorResponse
                    {
                        Code = 101,
                        Message = "کد ملی معتبر نمی باشد"
                    });
            }
            if (!(user.Firstname.Length > 0))
                return BadRequest(new ErrorResponse
                {
                    Code = 110,
                    Message = "نام درست وارد نشده است"
                });
            if (!(user.Lastname.Length > 0))
                return BadRequest(new ErrorResponse
                {
                    Code = 120,
                    Message = "نام خانوادگی درست وارد نشده است"
                });
            if (!(user.Password.Length > 0))
                return BadRequest(new ErrorResponse
                {
                    Code = 130,
                    Message = "رمز عبور درست وارد نشده است"
                });
            user.Mobile = ConvertPersianNumberToEnglish(user.Mobile);
            if (!Regex.Match(user.Mobile, @"(\+98|0)?9\d{9}").Success) //check if phone number is persian number
                return BadRequest(new ErrorResponse
                {
                    Code = 140,
                    Message = "شماره همراه درست وارد نشده است"
                });

            try
            {
                UserGroup userGroup = _context.UserGroups.Find(2);
                User newUser = new()
                {
                    Username = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Password = user.Password,
                    Mobile = user.Mobile,
                    UserGroup = userGroup,
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
            }
            catch (Exception)
            {
                return Problem("ساخت کاربر با مشکل مواجه شد");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public IActionResult Authenticate([FromBody] UserLogin user)
        {
            bool validate = CheckUsernameAndPassword(user);
            var token = _jwtAuthenticationManager.Authenticate(validate, user.Username);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        private bool CheckUsernameAndPassword(UserLogin user)
        {
            return _context.Users.Any(u => u.Username == user.Username && u.Password == user.Password);
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
