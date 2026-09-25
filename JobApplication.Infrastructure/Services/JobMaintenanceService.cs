using Application.Features.Jobs.Commands.AutoCloseStaleJobs;
using MediatR;
using JobApplication.Application.Features.Jobs.Commands.AutoCloseStaleJobs;
using JobApplication.Application.Interfaces;

namespace JobApplication.Infrastructure.Services
{
    public class JobMaintenanceService : IJobMaintenanceService
    {
        private readonly IMediator _mediator;

        public JobMaintenanceService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task AutoCloseStaleJobsAsync()
        {
            await _mediator.Send(new AutoCloseStaleJobsCommand());
        }
    }
}