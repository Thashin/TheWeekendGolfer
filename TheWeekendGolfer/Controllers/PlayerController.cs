using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Controllers;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        HandicapAccessLayer _handicapAccessLayer;
        PlayerAccessLayer _playerAccessLayer;


        public PlayerController(HandicapAccessLayer handicapAccessLayer, PlayerAccessLayer playerAccessLayer)
        {
            _handicapAccessLayer = handicapAccessLayer;
            _playerAccessLayer = playerAccessLayer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return Ok(_playerAccessLayer.GetAllPlayers());
            }
            catch
            {
                return BadRequest();
            }
        }



        [HttpGet]
        // GET: Player/Details/5
        public ActionResult Details(Guid id)
        {
            try
            {
                return Ok(_playerAccessLayer.GetPlayer(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: Player/Create
        [HttpPost]
   //     [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody]Player player)
        {
            Guid playerId = _playerAccessLayer.AddPlayer(player);
            if (playerId != null)
            {
                if(player.Handicap==null)
                {
                    return Ok();
                }
                else if(!_handicapAccessLayer.AddHandicap(new Handicap { Date = DateTime.Now, PlayerId = playerId, Value = player.Handicap.Value }))
                {
                    return BadRequest();
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        
        private Boolean Edit(Guid playerId,Decimal handicap)
        {
            Player player = _playerAccessLayer.GetPlayer(playerId);
            player.Handicap = handicap;
            player.Modified = DateTime.Now;
            return _playerAccessLayer.UpdatePlayer(player);
        }





        public Boolean RecalculateHandicap(Guid playerId)
        {
            IEnumerable<Handicap> handicaps = _handicapAccessLayer.GetOrderedHandicaps(playerId)
                                                                 .OrderBy(h=>h.Value);

            int roundsPlayed = handicaps.Count();
            Decimal newHandicap;

            if (roundsPlayed>=19)
            {
                newHandicap = handicaps.Take(8).Sum(h => h.Value) / 8;
            }
            else if (roundsPlayed >= 17)
            {
                newHandicap = handicaps.Take(7).Sum(h => h.Value) / 7;
            }
            else if (roundsPlayed >= 15)
            {
                newHandicap = handicaps.Take(6).Sum(h => h.Value) / 6;
            }
            else if (roundsPlayed >= 13)
            {
                newHandicap = handicaps.Take(5).Sum(h => h.Value) / 5;
            }
            else if (roundsPlayed >= 11)
            {
                newHandicap = handicaps.Take(4).Sum(h => h.Value) / 4;
            }
            else if (roundsPlayed >= 9)
            {
                newHandicap = handicaps.Take(3).Sum(h => h.Value) / 3;
            }
            else if (roundsPlayed >= 7)
            {
                newHandicap = handicaps.Take(2).Sum(h => h.Value) / 2;
            }
            else
            {
                newHandicap = handicaps.Take(1).Sum(h => h.Value) / 1;
            }

            return Edit(playerId, newHandicap);

        }

    }
}