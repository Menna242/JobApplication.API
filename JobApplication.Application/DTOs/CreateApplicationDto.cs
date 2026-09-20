using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTOs
{
    public class CreateApplicationDto
    {
        public int JobId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CvUrl { get; set; } = string.Empty;
    }
}