using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Controllers
{

    /// <summary>
    /// Used for creating one-way relationships with Players. A player's partner's handicap information are displayed on the player's home page
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class PartnerController : Controller
    {
        IPlayerAccessLayer _playerAccessLayer;
        IPartnerAccessLayer _partnerAccessLayer;
        private readonly UserManager<ApplicationUser> _userManager;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PartnerController(IPartnerAccessLayer partnerAccessLayer, IPlayerAccessLayer playerAccessLayer, UserManager<ApplicationUser> userManager)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _partnerAccessLayer = partnerAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _userManager = userManager;
        }

        /// <summary>
        /// Add a partner to a logged in user
        /// </summary>
        /// <param name="partner">The player to be added as a partner</param>
        /// <returns>Whether the partner was added successfully</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPartnerAsync([FromBody]Partner partner)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var playerIdTask =  _playerAccessLayer.GetPlayerByUserId(new Guid(user.Id));
                partner.PlayerId = playerIdTask.Id;
                await _partnerAccessLayer.AddPartner(partner);
            }
            return Ok(true);
        }


        /// <summary>
        /// Retrieves all partners for a given player Id
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>A list of partners</returns>
        [HttpGet]
        public IActionResult GetPartners(Guid playerId)
        {
            return Ok(_partnerAccessLayer.GetPartners(playerId));
        }

        /// <summary>
        /// Retrieves all players that are not currently partners of the logged in user
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>A list of partners</returns>
        [HttpGet]
        public IActionResult GetPotentialPartners(Guid playerId)
        {
            return Ok(_partnerAccessLayer.GetPotentialPartners(playerId));
        }

        /// <summary>
        /// Removes a partner
        /// </summary>
        /// <param name="Partner">The partner that should be removed</param>
        /// <returns>Whether the part wwas removed successfully or not</returns>
        [HttpPost]
        public async Task<IActionResult> RemovePartnerAsync([FromBody]Partner Partner)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Partner.PlayerId = _playerAccessLayer.GetPlayerByUserId(new Guid(user.Id)).Id;

                return Ok(await _partnerAccessLayer.DeletePartner(Partner));
            }
            return BadRequest();
        }
    }
}