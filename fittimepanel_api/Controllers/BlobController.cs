using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using FittimePanelApi.INotifications;
using FittimePanelApi.IRepository;
using FittimePanelApi.Models;
using FittimePanelApi.Models.Notifications;
using FittimePanelApi.Models.URLShortening;
using FittimePanelApi.Services;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase, IBlobController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlobController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IURLShortening _URLShortening;
        private readonly ISmsPanel _smsPanel;


        public BlobController(UserManager<User> userManager
                            , IUnitOfWork unitOfWork
                            , ILogger<BlobController> logger
                            , IMapper mapper
                            , IURLShortening URLShortening
                            , ISmsPanel smsPanel)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _URLShortening = URLShortening;
            _smsPanel = smsPanel;
        }

        [Authorize]
        [HttpPost("Avatar"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadAvatar(int? chunk, string name)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                string[] allowedImageTypes = new string[] { "image/jpeg", "image/png" };
                var fileUpload = Request.Form.Files[0];
                if (!allowedImageTypes.Contains(fileUpload.ContentType.ToLower()))
                {
                    _logger.LogError($"File format invalid for {nameof(UploadAvatar)}");
                    return BadRequest("File format invalid");
                }
                await fileUpload.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    UserBlob userBlob = await _unitOfWork.UserBlobs.Get(q => q.Key == "profile_picture" && q.User == currentUser);
                    if (userBlob == null)
                    {
                        userBlob = new UserBlob()
                        {
                            Key = "profile_picture",
                            User = currentUser,
                        };
                    }
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(memoryStream.ToArray())
                                        .Format(new WebPFormat())
                                        .Resize(new System.Drawing.Size(250, 250))
                                        .Quality(100)
                                        .Save(outStream);

                            userBlob.Value = outStream.ToArray();
                        }
                    }

                    if (userBlob.Id == 0)
                        await _unitOfWork.UserBlobs.Insert(userBlob);
                    else
                        _unitOfWork.UserBlobs.Update(userBlob);
                    await _unitOfWork.Save();

                    return Ok();
                }
                else
                {
                    _logger.LogError($"Size limit for {nameof(UploadAvatar)}");
                    return BadRequest("Size limited");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UploadAvatar)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [Authorize]
        [HttpPost("Exercise/Pdf"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadExercisePdf([FromForm] ExercisePdfBlobDTO exercisePdfBlobDTO)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                string[] allowedTypes = new string[] { "application/pdf" };
                var fileUpload = Request.Form.Files[0];
                if (!allowedTypes.Contains(fileUpload.ContentType.ToLower()))
                {
                    _logger.LogError($"File format invalid for {nameof(UploadAvatar)}");
                    return BadRequest("File format invalid");
                }
                await fileUpload.CopyToAsync(memoryStream);

                // Upload the file if less than 5 MB
                if (memoryStream.Length < 5242880)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    var excersie = await _unitOfWork.Exercises.Get(e => e.Id == exercisePdfBlobDTO.ExerciseId);
                    if (excersie == null)
                    {
                        _logger.LogError($"Exercise not exist {nameof(UploadExercisePdf)}");
                        return BadRequest($"Exercise not exist {nameof(UploadExercisePdf)}");
                    }
                    ExerciseDownload exerciseDownload = new ExerciseDownload();
                    exerciseDownload.ExerciseId = excersie.Id;
                    exerciseDownload.Value = memoryStream.ToArray();
                    exerciseDownload.Description = "دریافت برنامه";

                    await _unitOfWork.ExerciseDownloads.Insert(exerciseDownload);
                    await _unitOfWork.Save();

                    try
                    {
                        URLShorteningResponseDTO uRLShortening = (URLShorteningResponseDTO)await _URLShortening.ShortURL("exercise download", $"https://panel.fittimeteam.com/v1/api/Exercises/download/{exerciseDownload.Id}");
                        exerciseDownload.Uri = uRLShortening.Doc.Url;
                        _unitOfWork.ExerciseDownloads.Update(exerciseDownload);
                        await _unitOfWork.Save();
                        try
                        {
                            await _smsPanel.SendSMS(new SendSmsDTO()
                            {
                                To = new string[] { currentUser.PhoneNumber },
                                Text = String.Format("{0} عزیز! برنامه شما آماده دریافت می باشد. \n {1}", currentUser.FullName, exerciseDownload.Uri)
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Cant send sms");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Cant get short link");
                        exerciseDownload.Uri = $"https://fittimeteam.com/v1/api/Blob/Exercise/Download/{exerciseDownload.Id}";
                        _unitOfWork.ExerciseDownloads.Update(exerciseDownload);
                        await _unitOfWork.Save();
                    }

                    return Ok();
                }
                else
                {
                    _logger.LogError($"Size limit for {nameof(UploadAvatar)}");
                    return BadRequest("Size limited");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UploadAvatar)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/Blob/Exercise/Blob
        [Authorize]
        [HttpPost("Exercise/Blob"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrUpdateBlob([FromForm] ExerciseBlobDTO exerciseBlobDTO)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                string[] allowedImageTypes = new string[] { "image/jpeg", "image/png" };
                var fileUpload = exerciseBlobDTO.File;
                if (!allowedImageTypes.Contains(fileUpload.ContentType.ToLower()))
                {
                    _logger.LogError($"File format invalid for {nameof(AddOrUpdateBlob)}");
                    return BadRequest("File format invalid");
                }
                await fileUpload.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    ExerciseBlob exerciseBlob = new ExerciseBlob();
                    exerciseBlob = _mapper.Map<ExerciseBlob>(exerciseBlobDTO);
                    exerciseBlob.User = currentUser;
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(memoryStream.ToArray())
                                        .Format(new WebPFormat())
                                        .Quality(100)
                                        .Save(outStream);

                            exerciseBlob.Value = outStream.ToArray();
                        }
                    }

                    await _unitOfWork.ExerciseBlobs.Insert(exerciseBlob);
                    await _unitOfWork.Save();
                    var result = _mapper.Map<ExerciseBlobResponseDTO>(exerciseBlob);

                    return Ok(result);
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


    }
}
