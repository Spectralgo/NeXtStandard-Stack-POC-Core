using System.Threading.Tasks;
using NeXtStandardStack.Core.Api.Brokers.DateTimes;
using NeXtStandardStack.Core.Api.Brokers.Loggings;
using NeXtStandardStack.Core.Api.Brokers.Storages;
using NeXtStandardStack.Core.Api.Models.Players;

namespace NeXtStandardStack.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService : IPlayerService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PlayerService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Player> AddPlayerAsync(Player player) =>
            throw new System.NotImplementedException();
    }
}