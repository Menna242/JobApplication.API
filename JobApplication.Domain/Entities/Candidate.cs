using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Domain.Entities
{
    public class Candidate :BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; }
        public string CvUrl { get; set; }
    }
}
