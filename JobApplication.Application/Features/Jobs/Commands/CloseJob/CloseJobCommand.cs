using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.Jobs.Commands.CloseJob
{
    public class CloseJobCommand :IRequest<Unit>
    {
        public int JobId { get; set; }
        public string RecruiterId { get; set; } = string.Empty;
    }
}
