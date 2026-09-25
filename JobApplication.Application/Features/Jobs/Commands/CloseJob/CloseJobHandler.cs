using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.Jobs.Commands.CloseJob
{
    public class CloseJobHandler : IRequestHandler<CloseJobCommand, Unit>
    {
        private IRepository<Job> _jobRepository;

        public CloseJobHandler(IRepository<Job> repository)
        {
            _jobRepository = repository;
        }

        public async Task<Unit> Handle(CloseJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job == null)
                throw new Exception("Job not found");

            if (job.RecruiterId != request.RecruiterId)
                throw new Exception("You are not allowed to close this job");

            if (!job.IsActive)
                throw new Exception("This job is already closed");

            job.IsActive = false;
            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = request.RecruiterId;

            _jobRepository.Update(job);
            await _jobRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
