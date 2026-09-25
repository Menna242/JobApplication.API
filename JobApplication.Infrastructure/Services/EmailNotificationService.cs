using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace JobApplication.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IRepository<JobCandidateApplication> _applicationRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(
            IRepository<JobCandidateApplication> applicationRepository,
            IRepository<Candidate> candidateRepository,
            ILogger<EmailNotificationService> logger)
        {
            _applicationRepository = applicationRepository;
            _candidateRepository = candidateRepository;
            _logger = logger;
        }

        public async Task NotifyCandidate(int applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);

            if (application is null)
            {
                _logger.LogWarning("Application {ApplicationId} not found", applicationId);
                return;
            }

            var candidate = await _candidateRepository.GetByIdAsync(application.CandidateId);

            _logger.LogInformation(
                "Send Email: Notify candidate {CandidateName} that application {ApplicationId} status is now {Status}",
                candidate?.Name, applicationId, application.JobApplicationStatus);
        }


        public async Task NotifyRecruiter(int applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);

            if (application is null)
            {
                _logger.LogWarning("application {applicationId}is not found ", applicationId);
                return;
            }

            _logger.LogInformation("Send Email :  cadidate {CandidateId} has applied to {JobId} and applicationId is {applicationId}",
                application.CandidateId, application.JobId, applicationId);
        }
    }
}