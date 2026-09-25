using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.Applications.Commands.CancelApplication
{
    public class CancelApplicationCommand : IRequest<Unit>
    {
        public int ApplicationId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
