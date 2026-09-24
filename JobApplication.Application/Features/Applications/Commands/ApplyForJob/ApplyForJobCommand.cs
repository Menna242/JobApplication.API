using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.Applications.Commands.ApplyForJob
{
    public class ApplyForJobCommand : IRequest<int>
    {
        public int JobId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CvUrl { get; set; } = string.Empty;
    }
}
