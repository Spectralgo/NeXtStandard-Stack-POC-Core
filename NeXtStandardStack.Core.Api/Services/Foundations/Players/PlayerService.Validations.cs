using NeXtStandardStack.Core.Api.Models.Players;
using NeXtStandardStack.Core.Api.Models.Players.Exceptions;

namespace NeXtStandardStack.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private void ValidatePlayerOnAdd(Player player)
        {
            ValidatePlayerIsNotNull(player);
        }

        private static void ValidatePlayerIsNotNull(Player player)
        {
            if (player is null)
            {
                throw new NullPlayerException();
            }
        }
    }
}