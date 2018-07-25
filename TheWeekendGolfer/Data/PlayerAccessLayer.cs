using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Data
{
    public class PlayerAccessLayer
    {
        GolfDbContext _context;

        public PlayerAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public Player GetPlayer(Guid id)
        {
            try
            {
                return (Player)_context.Players.Select(s => s.Id.Equals(id));
            }
            catch
            {
                throw new Exception("Could not retrieve Player for " + id.ToString());
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

        public Boolean AddPlayer(Player Player)
        {
            try
            {
                _context.Players.Add(Player);

                return true;
            }
            catch
            {
                throw new Exception("Could not add Player for " + Player.Id.ToString());
            }
        }

        public Boolean UpdatePlayer(Player Player)
        {
            try
            {
                _context.Players.Update(Player);

                return true;
            }
            catch
            {
                throw new Exception("Could not update Player for " + Player.Id.ToString());
            }
        }

        public Boolean DeletePlayer(Player Player)
        {
            try
            {
                _context.Players.Remove(Player);

                return true;
            }
            catch
            {
                throw new Exception("Could not delete Player for " + Player.Id.ToString());
            }
        }
    }
}
