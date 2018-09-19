using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PartnerController : Controller
    {
        IPlayerAccessLayer _playerAccessLayer;
        IPartnerAccessLayer _partnerAccessLayer;
        private readonly UserManager<ApplicationUser> _userManager;

        public PartnerController(IPartnerAccessLayer partnerAccessLayer, IPlayerAccessLayer playerAccessLayer, UserManager<ApplicationUser> userManager)
        {
            _partnerAccessLayer = partnerAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddPartnerAsync([FromBody]Partner partner)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                var playerIdTask = await _playerAccessLayer.GetPlayerByUserId(new Guid(user.Id));
                partner.PlayerId = playerIdTask.Id;
                await _partnerAccessLayer.AddPartner(partner);
            }
            return Ok(true);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartners(Guid playerId)
        {
            return Ok(await _partnerAccessLayer.GetPartners(playerId));
        }

        [HttpGet]
        public async Task<IActionResult> GetPotentialPartners(Guid playerId)
        {
            return Ok(await _partnerAccessLayer.GetPotentialPartners(playerId));
        }

        [HttpPost]
        public async Task<IActionResult> RemovePartnerAsync([FromBody]Partner Partner)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var playerIdTask = await _playerAccessLayer.GetPlayerByUserId(new Guid(user.Id));
                Partner.PlayerId = playerIdTask.Id; 
                await _partnerAccessLayer.DeletePartner(Partner);
            }
                return Ok(true);
        }
    }
}