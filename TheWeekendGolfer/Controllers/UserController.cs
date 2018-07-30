using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private PlayerAccessLayer _playerAccessLayer;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PlayerAccessLayer playerAccessLayer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _playerAccessLayer = playerAccessLayer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]AddUser model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    var isSignedIn = await _signInManager.PasswordSignInAsync(model.Email,
         model.Password, false,false);
                   
                    model.Player.UserId = new Guid(user.Id);
                    _playerAccessLayer.AddPlayer(model.Player);
                }
                return Ok();
            }
            else
            {

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]User user)
        {
            if(ModelState.IsValid)
            {
                var isSignedIn = await _signInManager.PasswordSignInAsync(user.Email,user.Password
         , false, false);
                if (isSignedIn.Succeeded)
                {
                    return Ok("Login Successful");
                }
                else
                {
                    return Ok("Login Failed");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}