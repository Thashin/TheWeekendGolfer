using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IPartnerAccessLayer
    {
        IEnumerable<Player> GetPartners(Guid playerId);
        IEnumerable<Player> GetPotentialPartners(Guid playerId);

        Task<Boolean> AddPartner(Partner player);

        Task<Boolean> UpdatePartner(Partner partner);

        Task<Boolean> DeletePartner(Partner partner);
    }
}
