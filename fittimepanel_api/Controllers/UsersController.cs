using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FittimePanelApi;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using Microsoft.AspNetCore.Authorization;
using FittimePanelApi.IRepository;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using FittimePanelApi.Models;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, IUserController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly AppDb _context;

        public UsersController(UserManager<User> userManager
                                , IUnitOfWork unitOfWork
                                , ILogger<UsersController> logger
                                , IMapper mapper
                                , AppDb context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        // DELETE: api/Users/{id}
        [Authorize(Policy = "DeleteUsers")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var exercise = await _unitOfWork.Users.Get(q => q.Id == id.ToString());
                if (exercise == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteById)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Users.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }

        // GET: api/Users
        [Authorize(Policy = "GetAllUsers")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAll([FromQuery] QueryParamsDTO qp)
        {
            try
            {
                var users = _unitOfWork.Users.GetPage(
                    page: qp.Page,
                    itemsPerPage: qp.ItemsPerPage);
                await users.ToListAsync();
                var result = _mapper.Map<UserPageItemDTO>(users);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Users/Count
        [Authorize(Policy = "GetAllUsers")]
        [HttpGet("Count/{timespan}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CountRegistrations(string timespan)
        {
            try
            {
                IQueryable results;
                if (timespan == "month")
                    results = from user in _context.Users
                              where user.RegistrationDate > DateTime.Today.AddYears(-1)
                              group user by user.RegistrationDate.Month into day
                              select new
                              {
                                  Day = day.Key,
                                  Count = day.Count(),
                              };
                else
                    results = from user in _context.Users
                              where user.RegistrationDate > DateTime.Today.AddDays(-7)
                              group user by user.RegistrationDate.Date into day
                              select new
                              {
                                  Day = day.Key,
                                  Count = day.Count(),
                              };

                return Ok(results);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CountRegistrations)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Users/Search/{query}
        [Authorize(Policy = "GetAllUsers")]
        [HttpGet("Search/{query}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchAll([FromQuery] QueryParamsDTO qp, string query)
        {
            try
            {
                var users = _unitOfWork.Users.GetPage(u =>
                       EF.Functions.Like(u.UserName, $"%{query}%")
                    || EF.Functions.Like(u.FirstName, $"%{query}%")
                    || EF.Functions.Like(u.LastName, $"%{query}%"),
                        page: qp.Page,
                        itemsPerPage: qp.ItemsPerPage);
                await users.ToListAsync();
                var result = _mapper.Map<UserPageItemDTO>(users);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SearchAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Users/{id}
        [Authorize]
        [HttpGet("{id:Guid}", Name = "ReadUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadById(Guid id)
        {
            try
            {
                var user = await _unitOfWork.Users.Get(q => q.Id == id.ToString(), new List<string> {
                    "UserMetas"
                });
                var result = _mapper.Map<UserProfileDTO>(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        //// GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(Guid id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserExists(Guid id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}

        //private bool UsernameAndPasswordExists(string username, string password)
        //{
        //    return _context.Users.Any(u => u.Username == username && u.Password == password);
        //}

        //// POST: api/Users/Rules
        //[HttpPost("Rules")]
        //public async Task<ActionResult<UserRule>> PostRule(UserRule rule)
        //{
        //    _context.UserRules.Add(rule);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserRule", new { id = rule.Id }, rule);
        //}

        //// GET: api/Users/Groups/5
        //[HttpGet("Groups/{id}")]
        //public async Task<ActionResult<UserGroup>> GetGroup(int id)
        //{
        //    var userGroup = await _context.UserGroups.FindAsync(id);

        //    if (userGroup == null)
        //    {
        //        return NotFound();
        //    }

        //    return userGroup;
        //}

        //// POST: api/Users/Groups
        //[HttpPost("Groups")]
        //public async Task<ActionResult<UserGroup>> PostGroup(UserGroup group)
        //{
        //    _context.UserGroups.Add(group);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetGroup", new { id = group.Id }, group);
        //}

    }
}
