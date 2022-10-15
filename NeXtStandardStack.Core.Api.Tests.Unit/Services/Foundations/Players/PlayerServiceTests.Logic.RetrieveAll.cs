using System.Linq;
using FluentAssertions;
using Moq;
using NeXtStandardStack.Core.Api.Models.Players;
using Xunit;

namespace NeXtStandardStack.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public void ShouldReturnPlayers()
        {
            // given
            IQueryable<Player> randomPlayers = CreateRandomPlayers();
            IQueryable<Player> storagePlayers = randomPlayers;
            IQueryable<Player> expectedPlayers = storagePlayers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPlayers())
                    .Returns(storagePlayers);

            // when
            IQueryable<Player> actualPlayers =
                this.playerService.RetrieveAllPlayers();

            // then
            actualPlayers.Should().BeEquivalentTo(expectedPlayers);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPlayers(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}