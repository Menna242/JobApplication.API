using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;

namespace JobApplication.Application.Services
{
    public class ApplicationService 
    {
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRepository<Job> _jobRepository;
        private readonly IRepository<JobCandidateApplication> _applicationRepository;

        public ApplicationService(
            IRepository<Candidate> candidateRepository,
            IRepository<Job> jobRepository,
            IRepository<JobCandidateApplication> applicationRepository)
        {
            _candidateRepository = candidateRepository;
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task<int> ApplyAsync(string userId, CreateApplicationDto dto)
        {
            var job = await _jobRepository.GetByIdAsync(dto.JobId);
            if (job == null)
                throw new Exception("Job not found");

            if (!job.IsActive)
                throw new Exception("This job is closed");

            var candidates = await _candidateRepository.GetAllAsync();
            var candidate = candidates.FirstOrDefault(c => c.UserId == userId);

            if (candidate == null)
            {
                candidate = new Candidate
                {
                    UserId = userId,
                    Name = dto.Name,
                    CvUrl = dto.CvUrl
                };
                await _candidateRepository.InsertAsync(candidate);
                await _candidateRepository.SaveChangesAsync();
            }


            var applications = await _applicationRepository.GetAllAsync();
            var alreadyApplied = applications.Any(a => a.CandidateId == candidate.Id && a.JobId == dto.JobId);


            if (alreadyApplied)
                throw new Exception("You already applied for this job");

            var application = new JobCandidateApplication
            {
                CandidateId = candidate.Id,
                JobId = dto.JobId,
                JobApplicationStatus = JobApplicationStatus.Applied,
                AppliedAt = DateTime.UtcNow,
                StatusUpdatedAt = DateTime.UtcNow
            };

            await _applicationRepository.InsertAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return application.Id;
        }

        public async Task CancelAsync(string userId, int applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
                throw new Exception("Application not found");

            var candidate = await _candidateRepository.GetByIdAsync(application.CandidateId);
            if (candidate == null || candidate.UserId != userId)
                throw new Exception("You are not allowed to cancel this application");

            if (application.JobApplicationStatus != JobApplicationStatus.Applied &&
                    application.JobApplicationStatus != JobApplicationStatus.UnderReview)
            {
                throw new Exception("Only applications with status 'Applied' or 'UnderReview' can be cancelled");
            }

            application.JobApplicationStatus = JobApplicationStatus.Cancelled;
            application.StatusUpdatedAt = DateTime.UtcNow;
            application.CancelledAt = DateTime.UtcNow;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();
        }
    }
}