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
        PlayerAccessLayer _playerAccessLayer;
        PartnerAccessLayer _partnerAccessLayer;
        private readonly UserManager<ApplicationUser> _userManager;

        public PartnerController(PartnerAccessLayer partnerAccessLayer, PlayerAccessLayer playerAccessLayer, UserManager<ApplicationUser> userManager)
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
                partner.PlayerId = _playerAccessLayer.GetPlayerByUserId(new Guid(user.Id)).Id;
                _partnerAccessLayer.AddPartner(partner);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetPartners(Guid playerId)
        {
            return Ok(_partnerAccessLayer.GetPartners(playerId));
        }
    }
}