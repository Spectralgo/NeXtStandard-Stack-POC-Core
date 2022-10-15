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
        public async Task ShouldModifyPlayerAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Player randomPlayer = CreateRandomModifyPlayer(randomDateTimeOffset);
            Player inputPlayer = randomPlayer;
            Player storagePlayer = inputPlayer.DeepClone();
            storagePlayer.UpdatedDate = randomPlayer.CreatedDate;
            Player updatedPlayer = inputPlayer;
            Player expectedPlayer = updatedPlayer.DeepClone();
            Guid playerId = inputPlayer.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePlayerAsync(inputPlayer))
                    .ReturnsAsync(updatedPlayer);

            // when
            Player actualPlayer =
                await this.playerService.ModifyPlayerAsync(inputPlayer);

            // then
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(inputPlayer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}