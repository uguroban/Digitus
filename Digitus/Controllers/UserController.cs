using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Digitus.Dtos;
using Digitus.Models;
using Digitus.Services;
using Digitus.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Digitus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost($"CreateUser")]
        public async Task<IActionResult> CreateUser(SignupDto signupDto)
        {
            var response = await _userService.Register(signupDto);
            return CreateActionResult(response);
        }
        
        [HttpDelete($"DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var response = await _userService.DeleteUser(id);
            return CreateActionResult(response);
        }
        
        [HttpPost($"VerifyUser")]
        public async Task<IActionResult> CreateUser(string verifyCode)
        {
            var response = await _userService.Verify(verifyCode);
            return CreateActionResult(response);
        }

        [HttpGet($"ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await _userService.ForgotPassword(email);
            return CreateActionResult(response);
        }
        
        [HttpPut($"ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = await _userService.ResetPassword(resetPasswordDto);
            return CreateActionResult(response);
        }
        

        [HttpPost($"Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _userService.Login(loginDto);
            return CreateActionResult(response);
        }

        [HttpPut($"Logout")]
        public async Task<IActionResult> Logout(string email)
        {
            var response = await _userService.Logout(email);
            return CreateActionResult(response);
            
        }

        [HttpGet($"GetLoginUsers")]
        public async Task<IActionResult> GetLoginUsers()
        {
            var response = await _userService.GetLoginUsers();
            return CreateActionResult(response);
        }

        [HttpGet($"GetUserLogins")]
        public async Task<IActionResult> GetUserLogins(string email)
        {
            var response = await _userService.GetUserLogins(email);
            return CreateActionResult(response);
        }

        [HttpGet($"UserLoginTime")]
        public async Task<IActionResult> GetLoginTime(string email)
        {
            var response = await _userService.GetUserLoginTime(email);
            return CreateActionResult(response);
        }
        

    }
}
