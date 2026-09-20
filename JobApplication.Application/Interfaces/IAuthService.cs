using JobApplication.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<string> RegisterAsync(RegisterDto registerDto);
        public Task<string> LoginAsync(LoginDto loginDto);
    }
}
