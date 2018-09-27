using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IPlayerAccessLayer _playerAccessLayer;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPlayerAccessLayer playerAccessLayer)
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
                    return Ok(result.Succeeded);
                }
                else
                {

                    return BadRequest();
                }
            }
            else
            {

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                var isSignedIn = await _signInManager.PasswordSignInAsync(user.Email, user.Password
         , false, false);
                if (isSignedIn.Succeeded)
                {
                    return Ok("{\"Result\":\"Login Successful\"}");
                }
                else
                {
                    return Ok("{\"Result\":\"Login Failed\"}");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult isLoggedIn()
        {
            return Ok(User.Identity.IsAuthenticated);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayer()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return Ok(await _playerAccessLayer.GetPlayerByUserId(new Guid(user.Id)));
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("{\"Result\":\"Logout Successful\"}");
        }
    }
}