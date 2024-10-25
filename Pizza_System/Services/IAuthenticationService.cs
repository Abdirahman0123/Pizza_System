using Microsoft.AspNetCore.Identity;
using Pizza_System.Dtos;

namespace Pizza_System.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> Register(RegisterRequest request);
        //Task<Result<string>> Login(LoginRequest request);
        Task<string> Login(LoginRequest request);
    }
}
