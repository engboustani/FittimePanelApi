using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using FittimePanelApi.INotifications;
using FittimePanelApi.IRepository;
using FittimePanelApi.Models;
using FittimePanelApi.Models.Notifications;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase, IExercisesController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExercisesController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ISmsPanel _smsPanel;


        public ExercisesController(UserManager<User> userManager
                                , IUnitOfWork unitOfWork
                                , ILogger<ExercisesController> logger
                                , IMapper mapper
                                , ISmsPanel smsPanel)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _smsPanel = smsPanel;
        }

        // DELETE: api/Exercises/<uuid>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var exercise = await _unitOfWork.Exercises.Get(q => q.Id == id);
                if (exercise == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteById)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Exercises.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/Exercises
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> New([FromBody] CreateExerciseDTO createExerciseDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(New)}");
                return BadRequest(ModelState);
            }

            try
            {
                var exercise = _mapper.Map<Exercise>(createExerciseDTO);
                var currentUser = await _userManager.GetUserAsync(User);
                exercise.UserStudent = currentUser;
                var blobs = exercise.ExerciseBlobs;
                exercise.ExerciseBlobs = null;
                ICollection<ExerciseBlob> blobrecords = new Collection<ExerciseBlob>();
                //var type = await _unitOfWork.ExerciseTypes.Get(q => q.Id == createExerciseDTO.ExerciseType.Id);
                //exercise.ExerciseType = type;


                await _unitOfWork.Exercises.Insert(exercise);
                foreach (var blob in blobs)
                {
                    var blobrecord = await _unitOfWork.ExerciseBlobs.Get(b => b.Id == blob.Id);
                    blobrecord.Exercise = exercise;
                    _unitOfWork.ExerciseBlobs.Update(blobrecord);
                }
                await _unitOfWork.Save();

                try
                {
                    await _smsPanel.SendSMS(new SendSmsDTO()
                    {
                        To = new string[] { currentUser.PhoneNumber },
                        Text = String.Format("{0} عزیز! درخواست برنامه شما به موفقیت ایجاد شد. فیت تایم", currentUser.FullName)
                    });
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Cant send sms");
                }

                return CreatedAtRoute("ReadExerciseById", new { id = exercise.Id }, exercise);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(New)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/Exercises
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var exercises = await _unitOfWork.Exercises.GetAll(
                    expression: q => q.UserStudent == currentUser,
                    includes: new List<string> { "ExerciseType", "Payments" });
                var result = _mapper.Map<IList<ExerciseListItemDTO>>(exercises);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Exercises/All
        [Authorize(Policy = "GetAllExercises")]
        [HttpGet("All", Name = "ReadAllExercieses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAllExercieses([FromQuery] QueryParamsDTO qp)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var exercises = _unitOfWork.Exercises.GetPage(
                    includes: new List<string> { "ExerciseType", "Payments", "UserStudent" },
                    page: qp.Page,
                    itemsPerPage: qp.ItemsPerPage);
                await exercises.ToListAsync();
                var result = _mapper.Map<ExercisePageAllItemDTO>(exercises);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAllExercieses)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Exercises/<uuid>
        [Authorize]
        [HttpGet("{id:Guid}", Name = "ReadExerciseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadById(Guid id)
        {
            try
            {
                var exercise = await _unitOfWork.Exercises.Get(q => q.Id == id, new List<string> {
                    "ExerciseType", "ExerciseDownloads", "ExerciseMetas", "ExerciseBlobs"
                });
                var result = _mapper.Map<ExerciseDetailDTO>(exercise);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/Exercises/blob
        [Authorize]
        [HttpPost("blob"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrUpdateBlob([FromForm] ExerciseBlobDTO exerciseBlobDTO)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                string[] allowedImageTypes = new string[] { "image/jpeg", "image/png" };
                if (!allowedImageTypes.Contains(exerciseBlobDTO.File.ContentType.ToLower()))
                {
                    _logger.LogError($"File format invalid for {nameof(AddOrUpdateBlob)}");
                    return BadRequest("File format invalid");
                }
                await exerciseBlobDTO.File.CopyToAsync(memoryStream);

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

        // GET: api/Exercises/blob/{id}
        [HttpGet("blob/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadBlob(string id)
        {
            try
            {
                var exerciseBlob = await _unitOfWork.ExerciseBlobs.Get(b => b.Id == Guid.Parse(id));
                if (exerciseBlob == null)
                    return NoContent();
                var result = exerciseBlob.Value;

                return File(result, "image/webp");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadBlob)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Exercises/download/{id}
        [HttpGet("download/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadDownload(string id)
        {
            try
            {
                var exerciseDownload = await _unitOfWork.ExerciseDownloads.Get(b => b.Id == Guid.Parse(id));
                if (exerciseDownload == null)
                    return NoContent();
                var result = exerciseDownload.Value;

                return File(result, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadBlob)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Exercises/Types
        [Authorize]
        [HttpGet("Types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAllTypes()
        {
            try
            {
                var exerciseTypes = await _unitOfWork.ExerciseTypes.GetAll();
                var result = _mapper.Map<IList<ExerciseTypeDTO>>(exerciseTypes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAllTypes)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

    }
}
