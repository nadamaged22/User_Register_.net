using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Models;
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
        public UserController(UserManager<AppUser>userManager)
        {
            _userManger=userManager;
            
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
        
    }
}