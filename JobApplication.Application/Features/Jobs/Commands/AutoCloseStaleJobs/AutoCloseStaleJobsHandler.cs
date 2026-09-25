using JobApplication.Application.Features.Jobs.Commands.AutoCloseStaleJobs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;

namespace Application.Features.Jobs.Commands.AutoCloseStaleJobs
{
    public class AutoCloseStaleJobsHandler : IRequestHandler<AutoCloseStaleJobsCommand, int>
    {
        private readonly IRepository<Job> _jobRepository;
        private const int StaleThresholdDays = 30; 

        public AutoCloseStaleJobsHandler(IRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<int> Handle(AutoCloseStaleJobsCommand request, CancellationToken cancellationToken)
        {
            var allJobs = await _jobRepository.GetAllAsync();

            var staleJobs = allJobs
                .Where(j => j.IsActive && j.CreatedAt <= DateTime.UtcNow.AddDays(-StaleThresholdDays))
                .ToList();

            foreach (var job in staleJobs)
            {
                job.IsActive = false;
                job.ClosedAt = DateTime.UtcNow;
                job.ClosedBy = "System"; 
                _jobRepository.Update(job);
            }

            if (staleJobs.Any())
            {
                await _jobRepository.SaveChangesAsync();
            }

            return staleJobs.Count;
        }
    }
}