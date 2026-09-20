using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Domain.Entities
{
    public class Job:BaseEntity
    {
        public string Title { get; set; }
        public string Description  { get; set; }
        public bool IsActive { get; set; }
        public string RecruiterId { get; set; } = string.Empty;
        public DateTime? ClosedAt { get; set; }        
        public string? ClosedBy { get; set; }

    }
}
