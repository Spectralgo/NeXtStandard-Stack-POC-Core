using System;
using System.Linq;
using System.Threading.Tasks;
using NeXtStandardStack.Core.Api.Models.Players;

namespace NeXtStandardStack.Core.Api.Services.Foundations.Players
{
    public interface IPlayerService
    {
        ValueTask<Player> AddPlayerAsync(Player player);
        IQueryable<Player> RetrieveAllPlayers();
        ValueTask<Player> RetrievePlayerByIdAsync(Guid playerId);
    }
}