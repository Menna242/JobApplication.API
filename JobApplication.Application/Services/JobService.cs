using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using System.Threading.Tasks;

namespace JobApplication.Application.Services
{
    public class JobService
    {
        private readonly IRepository<Job> _jobRepository;

        public JobService(IRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<int> CreateAsync(CreateJobDto createJobDto, string recruiterId)
        {
            var job = new Job()
            {
                Title = createJobDto.Title,
                Description = createJobDto.Description,
                IsActive = true,
                RecruiterId = recruiterId
            };

            await _jobRepository.InsertAsync(job);
            await _jobRepository.SaveChangesAsync();

            return job.Id;
        }

        public async Task CloseAsync(int jobId, string recruiterId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null)
                throw new Exception("Job not found");

            if (job.RecruiterId != recruiterId)
                throw new Exception("You are not allowed to close this job");

            if (!job.IsActive)
                throw new Exception("This job is already closed");

            job.IsActive = false;
            job.ClosedAt = DateTime.UtcNow;     
            job.ClosedBy = recruiterId;

            _jobRepository.Update(job);
            await _jobRepository.SaveChangesAsync();
        }
    }
}