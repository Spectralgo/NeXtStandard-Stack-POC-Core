using System;
using System.Linq;
using System.Threading.Tasks;
using NeXtStandardStack.Core.Api.Models.Players;

namespace NeXtStandardStack.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Player> InsertPlayerAsync(Player player);
        IQueryable<Player> SelectAllPlayers();
        ValueTask<Player> SelectPlayerByIdAsync(Guid playerId);
        ValueTask<Player> UpdatePlayerAsync(Player player);
        ValueTask<Player> DeletePlayerAsync(Player player);
    }
}
