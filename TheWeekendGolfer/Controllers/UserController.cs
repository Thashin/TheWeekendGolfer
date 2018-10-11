using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Controllers
{
    /// <summary>
    /// Controls the CRUD operations for a user
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IPlayerAccessLayer _playerAccessLayer;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPlayerAccessLayer playerAccessLayer)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _playerAccessLayer = playerAccessLayer;
        }


        /// <summary>
        /// Create a user to login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

                    return Ok(JsonConvert.SerializeObject(result.Errors.Select(s => s).First().Description));
                }
            }
            else
            {

                return Ok(false);
            }
        }

        /// <summary>
        /// Used to login a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if a user is logged in
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IsLoggedIn()
        {
            return Ok(User.Identity.IsAuthenticated);
        }

        /// <summary>
        /// Retrieve the player which the logged in user is associated with.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPlayer()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return Ok(_playerAccessLayer.GetPlayerByUserId(new Guid(user.Id)));
            }
            return Ok(JsonConvert.SerializeObject(false));
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(JsonConvert.SerializeObject(true));
        }
    }
}