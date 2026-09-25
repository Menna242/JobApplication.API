using JobApplication.Application.Features.Applications.Commands.CancelApplication;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using MediatR;
using Hangfire;

namespace JobApplication.Application.Features.Applications.Commands.CancelApplication
{
    public class CancelApplicationHandler : IRequestHandler<CancelApplicationCommand, Unit>
    {
        private readonly IRepository<JobCandidateApplication> _applicationRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IBackgroundJobScheduler _jobScheduler;


        public CancelApplicationHandler(
            IRepository<JobCandidateApplication> applicationRepository,
            IRepository<Candidate> candidateRepository,
            IBackgroundJobScheduler jobScheduler)
        {
            _applicationRepository = applicationRepository;
            _candidateRepository = candidateRepository;
            _jobScheduler = jobScheduler;
        }

        public async Task<Unit> Handle(CancelApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByIdAsync(request.ApplicationId);
            if (application == null)
                throw new Exception("Application not found");

            var candidate = await _candidateRepository.GetByIdAsync(application.CandidateId);
            if (candidate == null || candidate.UserId != request.UserId)
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

            _jobScheduler.Enqueue<INotificationService>(x => x.NotifyCandidate(application.Id));

            return Unit.Value;
        }
    }
}