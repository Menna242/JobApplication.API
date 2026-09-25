using JobApplication.Application.DTOs;
using JobApplication.Application.Features.Applications.Commands.ApplyForJob;
using JobApplication.Application.Features.Applications.Commands.CancelApplication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Apply(CreateApplicationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new ApplyForJobCommand
            {
                JobId = dto.JobId,
                UserId = userId!,
                Name = dto.Name,
                CvUrl = dto.CvUrl
            };

            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new CancelApplicationCommand
            {
                ApplicationId = id,
                UserId = userId!
            };

            try
            {
                await _mediator.Send(command);
                return Ok(new { message = "Application cancelled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}