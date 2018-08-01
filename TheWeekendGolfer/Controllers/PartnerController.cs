using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PartnerController : Controller
    {
        PartnerAccessLayer _partnerAccessLayer;
        UserController _userController;

        public PartnerController(PartnerAccessLayer partnerAccessLayer, UserController userController)
        {
            _partnerAccessLayer = partnerAccessLayer;
            _userController = userController;
        }
        [HttpPost]
        public async Task<IActionResult> AddPartnerAsync([FromBody]Partner partner)
        {
            var player = await _userController.GetPlayer();
            partner.PlayerId = player.Value;
            _partnerAccessLayer.AddPartner(partner);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetPartners(Guid playerId)
        {
            return Ok(_partnerAccessLayer.GetPartners(playerId));
        }
    }
}