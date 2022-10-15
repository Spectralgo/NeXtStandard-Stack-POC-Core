using System.Threading.Tasks;
using NeXtStandardStack.Core.Api.Models.Players;
using NeXtStandardStack.Core.Api.Models.Players.Exceptions;
using Xeptions;

namespace NeXtStandardStack.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private delegate ValueTask<Player> ReturningPlayerFunction();

        private async ValueTask<Player> TryCatch(ReturningPlayerFunction returningPlayerFunction)
        {
            try
            {
                return await returningPlayerFunction();
            }
            catch (NullPlayerException nullPlayerException)
            {
                throw CreateAndLogValidationException(nullPlayerException);
            }
            catch (InvalidPlayerException invalidPlayerException)
            {
                throw CreateAndLogValidationException(invalidPlayerException);
            }
        }

        private PlayerValidationException CreateAndLogValidationException(Xeption exception)
        {
            var playerValidationException =
                new PlayerValidationException(exception);

            this.loggingBroker.LogError(playerValidationException);

            return playerValidationException;
        }
    }
}