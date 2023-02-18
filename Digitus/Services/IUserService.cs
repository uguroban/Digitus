using Digitus.Dtos;
using Digitus.Models;

namespace Digitus.Services;

public interface IUserService
{
    Task<Response<User>> Register(SignupDto signupDto);
    Task<Response<NoContent>> DeleteUser(string id);
    Task<Response<LoginDto>> Login(LoginDto loginDto);
    Task<Response<LoginDto>> Logout(string email);
    Task<Response<List<Login>>> GetLoginUsers();
    Task<Response<List<Login>>> GetUserLogins(string id);
    Task<Response<NoContent>> GetUserLoginTime(string email);
    Task<Response<NoContent>> Verify(string code);
    Task<Response<NoContent>> ForgotPassword(string email);
    Task<Response<NoContent>> ResetPassword(ResetPasswordDto resetPasswordDto);
}