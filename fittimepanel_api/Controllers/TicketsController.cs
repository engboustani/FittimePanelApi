﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FittimePanelApi;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using FittimePanelApi.IRepository;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FittimePanelApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase, ITicketsController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TicketsController(UserManager<User> userManager
                                ,IUnitOfWork unitOfWork
                                ,ILogger<TicketsController> logger
                                ,IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // DELETE: api/Tickets/<uuid>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.Get(q => q.Id == id);
                if (ticket == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteById)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Tickets.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }


        // POST: api/Tickets
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> New([FromBody] CreateTicketDTO createTicketDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(New)}");
                return BadRequest(ModelState);
            }

            try
            {
                var ticket = _mapper.Map<Ticket>(createTicketDTO);
                var currentUser = await _userManager.GetUserAsync(User);
                ticket.UserCreated = currentUser;
                ticket.TicketMessages.First().User = currentUser;
                await _unitOfWork.Tickets.Insert(ticket);
                await _unitOfWork.Save();

                return CreatedAtRoute("ReadById", new { id = ticket.Id }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(New)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/Tickets
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                var tickets = await _unitOfWork.Tickets.GetAll();
                var result = _mapper.Map<IList<TicketDTO>>(tickets);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Tickets/<uuid>
        [Authorize]
        [HttpGet("{id:Guid}", Name = "ReadById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadById(Guid id)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.Get(q => q.Id == id, new List<string> { "TicketMessages" , "TicketStatuses" });
                var result = _mapper.Map<TicketDTO>(ticket);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // PUT: api/Tickets/<uuid>
        [Authorize]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateNewTicketMessageDTO createNewTicketMessageDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
                return BadRequest(ModelState);
            }

            try
            {
                var ticket = await _unitOfWork.Tickets.Get(q => q.Id == id);
                if (ticket == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
                    return BadRequest("Submitted data is invalid");
                }

                var ticketMessage = _mapper.Map<TicketMessage>(createNewTicketMessageDTO);
                var currentUser = await _userManager.GetUserAsync(User);
                ticketMessage.User = currentUser;
                ticket.TicketMessages.Add(ticketMessage);
                _unitOfWork.Tickets.Update(ticket);
                await _unitOfWork.Save();

                return CreatedAtRoute("ReadById", new { id = ticket.Id }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Update)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
