using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    public interface IPlayerAccessLayer
    {
        Task<Player> GetPlayer(Guid id);


        Task<Player> GetPlayerByUserId(Guid id);


        Task<IEnumerable<Player>> GetAllPlayers();


        Task<Guid> AddPlayer(Player player);


        Task<Boolean> UpdatePlayer(Player player);


        Task<Boolean> DeletePlayer(Player player);

    }
}
