using System;
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
        public async Task ShouldThrowValidationExceptionOnModifyIfPlayerIsNullAndLogItAsync()
        {
            // given
            Player nullPlayer = null;
            var nullPlayerException = new NullPlayerException();

            var expectedPlayerValidationException =
                new PlayerValidationException(nullPlayerException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(nullPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfPlayerIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidPlayer = new Player
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidPlayerException = new InvalidPlayerException();

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
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Player.CreatedDate)}"
                });

            invalidPlayerException.AddData(
                key: nameof(Player.UpdatedByUserId),
                values: "Id is required");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(invalidPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    modifyPlayerTask.AsTask);

            //then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Player randomPlayer = CreateRandomPlayer(randomDateTimeOffset);
            Player invalidPlayer = randomPlayer;
            var invalidPlayerException = new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.UpdatedDate),
                values: $"Date is the same as {nameof(Player.CreatedDate)}");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(invalidPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(invalidPlayer.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Player randomPlayer = CreateRandomPlayer(randomDateTimeOffset);
            randomPlayer.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidPlayerException =
                new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.UpdatedDate),
                values: "Date is not recent");

            var expectedPlayerValidatonException =
                new PlayerValidationException(invalidPlayerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(randomPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerValidationException.Should().BeEquivalentTo(expectedPlayerValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}