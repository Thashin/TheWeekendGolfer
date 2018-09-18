using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public interface IPartnerAccessLayer
    {
        Task<IEnumerable<Player>> GetPartners(Guid playerId);
        Task<IEnumerable<Player>> GetPotentialPartners(Guid playerId);

        Task<Boolean> AddPartner(Partner player);

        Task<Boolean> UpdatePartner(Partner partner);

        Task<Boolean> DeletePartner(Partner partner);
    }
}
