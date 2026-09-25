using JobApplication.Application.DTOs;
using JobApplication.Application.Features.Jobs.Commands.CreateJob;
using JobApplication.Application.Features.Jobs.Commands.CloseJob;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> Create(CreateJobDto createJobDto)
        {
            var recruiterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = new CreateJobCommand
            {
                Title = createJobDto.Title,
                Description = createJobDto.Description,
                RecruiterId = recruiterId!
            };

            var id = await _mediator.Send(command);
            return Ok(new { id });
        }



        /// <summary>
        /// Closes a job so candidates can no longer apply to it.
        /// </summary>
        /// <param name="id">The ID of the job to close.</param>
        /// <returns>A success message if the job was closed.</returns>
        /// <response code="200">Job closed successfully.</response>
        /// <response code="400">Job not found, already closed, or you are not the owning recruiter.</response>
        /// <response code="401">Missing or invalid authentication token.</response>
        /// <response code="403">You are not a Recruiter.</response>
        [HttpPut("{id}/close")]
        [Authorize(Roles = "Recruiter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Close(int id)
        {
            var recruiterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = new CloseJobCommand
            {
                JobId = id,
                RecruiterId = recruiterId!
            };

            try
            {
                await _mediator.Send(command);
                return Ok(new { message = "Job closed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
