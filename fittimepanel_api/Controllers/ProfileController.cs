using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using FittimePanelApi.IRepository;
using FittimePanelApi.Models;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase, IProfileController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TicketsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager
                                , IUnitOfWork unitOfWork
                                , ILogger<TicketsController> logger
                                , IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("blob/{key}", Name = "ReadBlobByKey")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadBlob(string key)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var userBlob = await _unitOfWork.UserBlobs.Get(q => q.User == currentUser && q.Key == key);
                var result = _mapper.Map<UserBlobResponseDTO>(userBlob);
                string base64String = Convert.ToBase64String(userBlob.Value, 0, userBlob.Value.Length);
                result.File = "data:image/webp;base64," + base64String;

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadBlob)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [Authorize]
        [HttpPost("blob"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrUpdateBlob([FromForm] UserBlobDTO userBlobDTO)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                string[] allowedImageTypes = new string[] { "image/jpeg", "image/png" };
                if (!allowedImageTypes.Contains(userBlobDTO.File.ContentType.ToLower()))
                {
                    _logger.LogError($"File format invalid for {nameof(AddOrUpdateBlob)}");
                    return BadRequest("File format invalid");
                }
                await userBlobDTO.File.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    UserBlob userBlob = await _unitOfWork.UserBlobs.Get(q => q.Key == userBlobDTO.Key && q.User == currentUser);
                    if(userBlob == null)
                    {
                        userBlob = _mapper.Map<UserBlob>(userBlobDTO);
                        userBlob.User = currentUser;
                    }
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(memoryStream.ToArray())
                                        .Format(new WebPFormat())
                                        .Quality(100)
                                        .Save(outStream);

                            userBlob.Value = outStream.ToArray();
                        }
                    }

                    if(userBlob.Id == 0)
                        await _unitOfWork.UserBlobs.Insert(userBlob);
                    else
                        _unitOfWork.UserBlobs.Update(userBlob);
                    await _unitOfWork.Save();

                    return Ok();
                }
                else
                {
                    _logger.LogError($"Size limit for {nameof(AddOrUpdateBlob)}");
                    return BadRequest("Size limited");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(AddOrUpdateBlob)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [Authorize]
        [HttpPost("meta"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMeta()
        {
            return Ok();
        }

        [HttpGet("avatar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAvatar(string id)
        {
            try
            {
                var userBlob = await _unitOfWork.UserBlobs.Get(q => q.User.Id == id && q.Key == "profile_picture");
                if (userBlob == null)
                    return NoContent();
                var result = userBlob.Value;

                return File(result, "image/webp");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadBlob)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var user = await _unitOfWork.Users.Get(q => q == currentUser, new List<string> { "UserMetas" });
                var result = _mapper.Map<UserProfileDTO>(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetProfile)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile(UserProfileDTO userProfileDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateProfile)}");
                return BadRequest(ModelState);
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var userMetas = await _unitOfWork.UserMetas.GetAll(m => m.User == currentUser);

                _mapper.Map(userProfileDTO, currentUser);
                foreach (var userMeta in currentUser.UserMetas)
                {
                    var oldUserMeta = userMetas
                        .SingleOrDefault(m => m.Key == userMeta.Key);
                    if (oldUserMeta != null)
                    {
                        userMeta.Id = oldUserMeta.Id;
                    }
                }
                //_unitOfWork.Users.Update(user);
                //await _unitOfWork.Save();
                await _userManager.UpdateAsync(currentUser);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateProfile)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }
    }
}
