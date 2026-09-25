using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyCandidate(int applicationId);

        Task NotifyRecruiter(int applicationId);
    }
}
