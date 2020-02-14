using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWeekendGolfer.Controllers;
using TheWeekendGolfer.Data;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Web.Data;

namespace TheWeekendGolfer.Web.Controllers
{
    /// <summary>
    /// Manages the CRUD operations for Players
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        IHandicapAccessLayer _handicapAccessLayer;
        IPlayerAccessLayer _playerAccessLayer;
        ICourseAccessLayer _courseAccessLayer;
        IScoreAccessLayer _scoreAccessLayer;
        IGolfRoundAccessLayer _golfRoundAccessLayer;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PlayerController(UserManager<ApplicationUser> userManager,IHandicapAccessLayer handicapAccessLayer, IPlayerAccessLayer playerAccessLayer, ICourseAccessLayer courseAccessLayer, IScoreAccessLayer scoreAccessLayer, IGolfRoundAccessLayer golfRoundAccessLayer)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _userManager = userManager;
            _handicapAccessLayer = handicapAccessLayer;
            _playerAccessLayer = playerAccessLayer;
            _courseAccessLayer = courseAccessLayer;
            _scoreAccessLayer = scoreAccessLayer;
            _golfRoundAccessLayer = golfRoundAccessLayer;
        }



        /// <summary>
        /// Retrieves all handicaps for a player sorted by most recent first
        /// </summary>
        /// <param name="PlayerId">Player whose handicaps is to be retrieved</param>
        /// <returns>An ordered list of handicaps</returns>
        [HttpGet]
        public IActionResult GetOrderedHandicaps(Guid PlayerId)
        {
            return Ok(_handicapAccessLayer.GetOrderedHandicaps(PlayerId));
        }

        /// <summary>
        /// Retrieves all players currently in the system
        /// </summary>
        /// <returns>A list of players</returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return Ok( _playerAccessLayer.GetAllPlayers());
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Retrieves current handicap details for a player
        /// </summary>
        /// <param name="PlayerId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Details(Guid PlayerId)
        {
            try
            {
                return Ok(_playerAccessLayer.GetPlayer(PlayerId));
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Retrieves current handicap details for a player by player name
        /// </summary>
        /// <param name="PlayerName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DetailsByPlayerName(String PlayerName)
        {
            try
            {
                return Ok(_playerAccessLayer.GetPlayerByName(PlayerName));
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates a player
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Whether the player was created successfully</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Player player)
        {
            var user = await _userManager.GetUserAsync(User);
            player.UserId = new Guid(user.Id);
            Guid playerId = await _playerAccessLayer.AddPlayer(player);
            if (playerId != null)
            {
                if (player.Handicap == null)
                {
                    return Ok(playerId);
                }
                else if (!await _handicapAccessLayer.AddHandicap(
                        new Handicap
                        {
                            Date = DateTime.Now,
                            PlayerId = playerId,
                            Value = player.Handicap.Value,
                            CurrentHandicap = player.Handicap.Value
                        }))
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(playerId);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Retrieve all the courses that a player has played at and the number of times he/she has played each course
        /// </summary>
        /// <param name="PlayerId"></param>
        /// <returns>A list of courses with the number of times the course has been played</returns>
        [HttpGet]
        public IActionResult GetAllPlayerRoundCourses(Guid PlayerId)
        {
            var rounds = _scoreAccessLayer.GetAllPlayerScores(PlayerId);
            var courses = _golfRoundAccessLayer.GetAllGolfRoundCourseIds(rounds.Select(s => s.GolfRoundId).ToList());
            return Ok(_courseAccessLayer.GetCourseStats(courses.ToList()));
        }

    }
}