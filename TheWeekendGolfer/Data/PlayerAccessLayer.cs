using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;
using TheWeekendGolfer.Data.ApplicationUserDB;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PlayerAccessLayer : IPlayerAccessLayer
    {
        GolfDbContext _context;
        ApplicationDbContext _userContext;

        public PlayerAccessLayer(GolfDbContext context, ApplicationDbContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public Player GetPlayer(Guid id)
        {
            try
            {
                return _context.Players.Where(s => s.Id.Equals(id)).First();
            }
            catch
            {
                throw new Exception("Could not retrieve Player for " + id.ToString());
            }
        }

        public Player GetPlayerByUserId(Guid id)
        {
            try
            {
                return _context.Players.Where(s => s.UserId.Equals(id)).First();
            }
            catch
            {
                throw new Exception("Could not retrieve Player for " + id.ToString());
            }
        }

        public Player GetPlayerByName(string playerName)
        {
            try
            {

                return _context.Players.Where(p => playerName.Equals(p.FirstName + " " + p.LastName)).First();
            }
            catch
            {
                throw new Exception("Could not retrieve Player for " + playerName);
            }
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            try
            {
                return _context.Players.Select(s => s);
            }
            catch
            {
                throw new Exception("Could not retrieve all Players");
            }
        }

        public async Task<Guid> AddPlayer(Player player)
        {
            if (_userContext.Users.Where(u => (new Guid(u.Id)).Equals(player.UserId)).Count() > 0)
            {
                try
                {
                    player.Modified = DateTime.Now;
                    _context.Players.Add(player);
                    await _context.SaveChangesAsync();
                    return player.Id;
                }
                catch
                {
                    throw new Exception("Could not add Player for " + player.Id.ToString());
                }
            }
            else
            {
                throw new Exception("User does not exist " + player.Id.ToString());
            }

        }

        public async Task<Boolean> UpdatePlayer(Player player)
        {
            try
            {
                _context.Players.Update(player);
                return (0 < await _context.SaveChangesAsync());
            }
            catch
            {
                throw new Exception("Could not update Player for " + player.Id.ToString());
            }
        }

        public async Task<Boolean> DeletePlayer(Player player)
        {
            try
            {
                _context.Players.Remove(player);
                return (0 < await _context.SaveChangesAsync());
                
            }
            catch
            {
                throw new Exception("Could not delete Player for " + player.Id.ToString());
            }
        }
    }
}
