using System;
using Moq;
using NeXtStandardStack.Core.Api.Brokers.DateTimes;
using NeXtStandardStack.Core.Api.Brokers.Loggings;
using NeXtStandardStack.Core.Api.Brokers.Storages;
using NeXtStandardStack.Core.Api.Models.Players;
using NeXtStandardStack.Core.Api.Services.Foundations.Players;
using Tynamix.ObjectFiller;

namespace NeXtStandardStack.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPlayerService playerService;

        public PlayerServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.playerService = new PlayerService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Player CreateRandomPlayer(DateTimeOffset dateTimeOffset) =>
            CreatePlayerFiller(dateTimeOffset).Create();

        private static Filler<Player> CreatePlayerFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<Player>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(player => player.CreatedByUserId).Use(userId)
                .OnProperty(player => player.UpdatedByUserId).Use(userId);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}