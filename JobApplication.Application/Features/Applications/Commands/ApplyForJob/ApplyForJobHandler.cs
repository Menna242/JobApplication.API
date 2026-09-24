using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using MediatR;

namespace JobApplication.Application.Features.Applications.Commands.ApplyForJob
{
    public class ApplyForJobHandler : IRequestHandler<ApplyForJobCommand, int>
    {
        private readonly IRepository<Job> _jobRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRepository<JobCandidateApplication> _applicationRepository;

        public ApplyForJobHandler(
            IRepository<Job> jobRepository,
            IRepository<Candidate> candidateRepository,
            IRepository<JobCandidateApplication> applicationRepository)
        {
            _jobRepository = jobRepository;
            _candidateRepository = candidateRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task<int> Handle(ApplyForJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job == null)
                throw new Exception("Job not found");

            if (!job.IsActive)
                throw new Exception("This job is closed");

            var candidates = await _candidateRepository.GetAllAsync();
            var candidate = candidates.FirstOrDefault(c => c.UserId == request.UserId);

            if (candidate == null)
            {
                candidate = new Candidate
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    CvUrl = request.CvUrl
                };
                await _candidateRepository.InsertAsync(candidate);
                await _candidateRepository.SaveChangesAsync();
            }

            var applications = await _applicationRepository.GetAllAsync();
            var alreadyApplied = applications.Any(a => a.CandidateId == candidate.Id && a.JobId == request.JobId);

            if (alreadyApplied)
                throw new Exception("You already applied for this job");

            var application = new JobCandidateApplication
            {
                CandidateId = candidate.Id,
                JobId = request.JobId,
                JobApplicationStatus = JobApplicationStatus.Applied,
                AppliedAt = DateTime.UtcNow,
                StatusUpdatedAt = DateTime.UtcNow
            };

            await _applicationRepository.InsertAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return application.Id;
        }
    }
}