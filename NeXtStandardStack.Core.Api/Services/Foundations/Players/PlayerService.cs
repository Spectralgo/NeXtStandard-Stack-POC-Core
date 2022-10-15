using System;
using System.Linq;
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
            TryCatch(async () =>
            {
                ValidatePlayerOnAdd(player);

                return await this.storageBroker.InsertPlayerAsync(player);
            });

        public IQueryable<Player> RetrieveAllPlayers() =>
            TryCatch(() => this.storageBroker.SelectAllPlayers());

        public ValueTask<Player> RetrievePlayerByIdAsync(Guid playerId) =>
            TryCatch(async () =>
            {
                ValidatePlayerId(playerId);

                Player maybePlayer = await this.storageBroker
                    .SelectPlayerByIdAsync(playerId);

                ValidateStoragePlayer(maybePlayer, playerId);

                return maybePlayer;
            });

        public ValueTask<Player> ModifyPlayerAsync(Player player) =>
            TryCatch(async () =>
            {
                ValidatePlayerOnModify(player);

                Player maybePlayer =
                    await this.storageBroker.SelectPlayerByIdAsync(player.Id);

                ValidateStoragePlayer(maybePlayer, player.Id);
                ValidateAgainstStoragePlayerOnModify(inputPlayer: player, storagePlayer: maybePlayer);

                return await this.storageBroker.UpdatePlayerAsync(player);
            });

        public ValueTask<Player> RemovePlayerByIdAsync(Guid playerId) =>
            TryCatch(async () =>
            {
                ValidatePlayerId(playerId);

                Player maybePlayer = await this.storageBroker
                    .SelectPlayerByIdAsync(playerId);

                return await this.storageBroker.DeletePlayerAsync(maybePlayer);
            });
    }
}