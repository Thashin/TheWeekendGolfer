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

        public Boolean AddPlayer(Player player)
        {
            try
            {
                player.Modified = DateTime.Now;
                _context.Players.Add(player);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not add Player for " + player.Id.ToString());
            }
        }

        public Boolean UpdatePlayer(Player player)
        {
            try
            {
                _context.Players.Update(player);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not update Player for " + player.Id.ToString());
            }
        }

        public Boolean DeletePlayer(Player player)
        {
            try
            {
                _context.Players.Remove(player);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not delete Player for " + player.Id.ToString());
            }
        }
    }
}
