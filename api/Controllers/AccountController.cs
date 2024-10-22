using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser>_userManger;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signin;
        public AccountController(UserManager<AppUser>userManager,ITokenService tokenService,SignInManager<AppUser>signInManager)
        {
            _userManger=userManager;
            _tokenService=tokenService;
            _signin=signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult>Register([FromBody]RegisterDto register){
            try
            {
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var appUser=new AppUser{
                    UserName=register.Username,
                    Email=register.Email
                };
                var createdUser=await _userManger.CreateAsync(appUser,register.Password);
                if(createdUser.Succeeded){
                    var roleResult=await _userManger.AddToRoleAsync(appUser,"User");
                    if(roleResult.Succeeded){
                        return Ok ("User Added Successfully!");
                    }else{
                        return StatusCode(500,roleResult.Errors);
                    }
                }else{
                    return StatusCode(500,createdUser.Errors);
                }

            }
            catch(Exception e)
            {
                return StatusCode(500,e);

            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDto login){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);

            }
            var user =await _userManger.Users.FirstOrDefaultAsync(x => x.UserName==login.UserName.ToLower());
            if(user==null){
                return Unauthorized("Invalid Username!");
            }
            var result=await _signin.CheckPasswordSignInAsync(user,login.Password,false);
            if(!result.Succeeded){
                return Unauthorized("Please Check Your Username or Your Password");

            }
            return Ok(
                new NewUserDto{
                    UserName=user.UserName,
                    Email=user.Email,
                    Token=_tokenService.CreateToken(user)
                }
            );
        }
        
    }
}