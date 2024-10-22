using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly ICloudinaryService _clodinaryService;
        public UserController(UserManager<AppUser>userManager,ICloudinaryService cloudinaryService )
        {
            _userManger=userManager;
            _clodinaryService=cloudinaryService;
            
        }
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetProfile(){
            var Username=User.GetUsername();
            if(string.IsNullOrEmpty(Username)){
                return Unauthorized("Invalid Token");
            }
            var appUser=await _userManger.FindByNameAsync(Username);
            if(appUser==null){
                return NotFound("User Not Found!");
            }
            var userProfile=new
            {
                appUser.Id,
                appUser.UserName,
                appUser.Email
            };
            return Ok(userProfile);


        }
        [HttpPost("api/uploadPhoto")]
        [Authorize]
        
        public async Task<IActionResult> AddProfilePhoto(IFormFile file){
            var user=await _userManger.FindByNameAsync(User.GetUsername());
            if(user==null){
                return NotFound("User Not_Found!");
        
            }
            var result=await _clodinaryService.AddPhotoAsync(file);
            if(result.Error!=null){
                return BadRequest(result.Error.Message);
            }
            user.ProfilePhotoUrl=result.SecureUrl.AbsoluteUri;
            user.publicId=result.PublicId;
            var uploadResult=await _userManger.UpdateAsync(user);
            if(!uploadResult.Succeeded){
                return BadRequest("Faild to update user profile picture!");
            }
            return Ok(
                new {
                    user.ProfilePhotoUrl
                }
            );

        }
        
    }
}