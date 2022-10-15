using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NeXtStandardStack.Core.Api.Models.Players;
using Xunit;

namespace NeXtStandardStack.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldAddPlayerAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Player randomPlayer = CreateRandomPlayer(randomDateTimeOffset);
            Player inputPlayer = randomPlayer;
            Player storagePlayer = inputPlayer;
            Player expectedPlayer = storagePlayer.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPlayerAsync(inputPlayer))
                    .ReturnsAsync(storagePlayer);

            // when
            Player actualPlayer = await this.playerService
                .AddPlayerAsync(inputPlayer);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(inputPlayer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}