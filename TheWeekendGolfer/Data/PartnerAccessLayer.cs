using System;
using System.Collections.Generic;
using System.Linq;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public class PartnerAccessLayer
    {
        GolfDbContext _context;

        public PartnerAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Guid> GetPartners(Guid playerId)
        {
            try
            {
                return _context.Partners.Where(s => s.PlayerId.Equals(playerId)).Select(p => p.PartnerId);
            }
            catch
            {
                throw new Exception("Could not retrieve Partner for " + playerId);
            }
        }

        /// <summary>
        /// Returns all players that are not currently partners with given player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public IEnumerable<Player> GetPotentialPartners(Guid playerId)
        {
            try
            {
                var partners = _context.Partners.Where(s => s.PlayerId.Equals(playerId)).Select(p => p.PartnerId);
                return _context.Players.Where(p => !p.Id.Equals(playerId) && !partners.Contains(p.Id));
            }
            catch
            {
                throw new Exception("Could not retrieve potential Partner for " + playerId);
            }
        }

        public Boolean AddPartner(Partner player)
        {
            try
            {
                if (!checkPartner(player))
                {
                    _context.Partners.Add(player);
                    _context.SaveChanges();
                }
                return true;
            }
            catch
            {
                throw new Exception("Could not add Partner for " + player.Id.ToString());
            }
        }

        /// <summary>
        /// Ensures that user cannot add the same partner twice 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        private Boolean checkPartner(Partner partner)
        {
            try
            {
                return _context.Partners.Where(p => p.PlayerId.Equals(partner.PlayerId) &&
                                                    p.PartnerId.Equals(partner.PartnerId))
                                        .Any();
            }
            catch
            {
                throw new Exception("Could not check Partner for " + partner.Id.ToString());
            }
        }

        public Boolean UpdatePartner(Partner course)
        {
            try
            {
                _context.Partners.Update(course);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not update Partner for " + course.Id.ToString());
            }
        }

        public Boolean DeletePartner(Partner course)
        {
            try
            {
                _context.Partners.Remove(course);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not delete Partner for " + course.Id.ToString());
            }
        }
    }
}
