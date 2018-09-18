using System;
using System.Collections.Generic;
using System.Linq;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Models;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Data
{
    public class PartnerAccessLayer:IPartnerAccessLayer
    {
        GolfDbContext _context;

        public PartnerAccessLayer(GolfDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Player>> GetPartners(Guid playerId)
        {
            try
            {
                var partnerIds = _context.Partners.Where(s => s.PlayerId.Equals(playerId)).Select(p => p.PartnerId);
                return _context.Players.Where(p => partnerIds.Contains(p.Id)).Select(p => new Player{ Id= p.Id,FirstName= p.FirstName, LastName = p.LastName }).ToList();
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
        public async Task<IEnumerable<Player>> GetPotentialPartners(Guid playerId)
        {
            try
            {
                var player = _context.Partners.Where(s => s.PlayerId.Equals(playerId));
                if (0<player.Count())
                {
                    var partners = player.Select(p => p.PartnerId);
                    return _context.Players.Where(p => !p.Id.Equals(playerId) && !partners.Contains(p.Id)).ToList();
                }
                return new List<Player>();
            }
            catch
            {
                throw new Exception("Could not retrieve potential Partner for " + playerId);
            }
        }

        public async Task<Boolean> AddPartner(Partner player)
        {
            try
            {
                if (!CheckPartner(player))
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
        private Boolean CheckPartner(Partner partner)
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

        public async Task<Boolean> UpdatePartner(Partner partner)
        {
            try
            {
                _context.Partners.Update(partner);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not update Partner for " + partner.Id.ToString());
            }
        }

        public async Task<Boolean> DeletePartner(Partner partner)
        {
            try
            {
                partner = _context.Partners.Where(p => partner.PartnerId.Equals(p.PartnerId) && partner.PlayerId.Equals(p.PlayerId)).FirstOrDefault();
                _context.Remove(partner);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not delete Partner for " + partner.Id.ToString());
            }
        }
    }
}
