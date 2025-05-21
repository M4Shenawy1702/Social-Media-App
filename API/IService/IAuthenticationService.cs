using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.AuthenticationDtos;

namespace API.IService
{
    public interface IAuthenticationService
    {
        Task<UserResponse> LoginAsync(LoginRequest request);
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task SendEmailVerificationCodeAsync(string userId);
        Task ConfirmEmailWithCodeAsync(string userId, string code);
    }
}