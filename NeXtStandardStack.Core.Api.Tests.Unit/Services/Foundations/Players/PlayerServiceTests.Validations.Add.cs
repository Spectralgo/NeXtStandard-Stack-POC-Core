using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NeXtStandardStack.Core.Api.Models.Players;
using NeXtStandardStack.Core.Api.Models.Players.Exceptions;
using Xunit;

namespace NeXtStandardStack.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPlayerIsNullAndLogItAsync()
        {
            // given
            Player nullPlayer = null;

            var nullPlayerException =
                new NullPlayerException();

            var expectedPlayerValidationException =
                new PlayerValidationException(nullPlayerException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(nullPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    addPlayerTask.AsTask);

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfPlayerIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidPlayer = new Player
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidPlayerException =
                new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.Id),
                values: "Id is required");

            //invalidPlayerException.AddData(
            //    key: nameof(Player.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Player model

            invalidPlayerException.AddData(
                key: nameof(Player.CreatedDate),
                values: "Date is required");

            invalidPlayerException.AddData(
                key: nameof(Player.CreatedByUserId),
                values: "Id is required");

            invalidPlayerException.AddData(
                key: nameof(Player.UpdatedDate),
                values: "Date is required");

            invalidPlayerException.AddData(
                key: nameof(Player.UpdatedByUserId),
                values: "Id is required");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(invalidPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    addPlayerTask.AsTask);

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}