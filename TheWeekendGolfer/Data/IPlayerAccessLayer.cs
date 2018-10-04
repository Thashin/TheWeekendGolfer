using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IPlayerAccessLayer
    {
        Player GetPlayer(Guid id);


        Player GetPlayerByUserId(Guid id);


        IEnumerable<Player> GetAllPlayers();


        Task<Guid> AddPlayer(Player player);


        Task<Boolean> UpdatePlayer(Player player);


        Task<Boolean> DeletePlayer(Player player);

    }
}
