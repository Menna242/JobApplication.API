using JobApplication.Application.DTOs;
using JobApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobService _JobService;

        public JobsController(JobService jobService)
        {
            _JobService = jobService;
        }

        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> Create(CreateJobDto createJobDto)
        {
            var recruiterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var id = await _JobService.CreateAsync(createJobDto, recruiterId!);
            return Ok(new
            {
                id = id 
            }); 
        }


        [HttpPut("{id}/close")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> Close(int id)
        {
            var recruiterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _JobService.CloseAsync(id, recruiterId!);
                return Ok(new { message = "Job closed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
