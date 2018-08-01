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
                return _context.Partners.Where(s => s.PlayerId.Equals(playerId)).Select(p=>p.PartnerId) ;
            }
            catch
            {
                throw new Exception("Could not retrieve Partner for " + playerId);
            }
        }

        public Boolean AddPartner(Partner player)
        {
            try
            {
                _context.Partners.Add(player);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw new Exception("Could not add Partner for " + player.Id.ToString());
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
