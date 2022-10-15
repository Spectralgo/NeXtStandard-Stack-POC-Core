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
                await Assert.ThrowsAsync<PlayerValidationException>(() =>
                    addPlayerTask.AsTask());

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
                await Assert.ThrowsAsync<PlayerValidationException>(() =>
                    addPlayerTask.AsTask());

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Player randomPlayer = CreateRandomPlayer(randomDateTimeOffset);
            Player invalidPlayer = randomPlayer;

            invalidPlayer.UpdatedDate =
                invalidPlayer.CreatedDate.AddDays(randomNumber);

            var invalidPlayerException = new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.UpdatedDate),
                values: $"Date is not the same as {nameof(Player.CreatedDate)}");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(invalidPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(() =>
                    addPlayerTask.AsTask());

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUserIdsIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Player randomPlayer = CreateRandomPlayer(randomDateTimeOffset);
            Player invalidPlayer = randomPlayer;
            invalidPlayer.UpdatedByUserId = Guid.NewGuid();

            var invalidPlayerException =
                new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.UpdatedByUserId),
                values: $"Id is not the same as {nameof(Player.CreatedByUserId)}");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(invalidPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(() =>
                    addPlayerTask.AsTask());

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

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

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            Player randomPlayer = CreateRandomPlayer(invalidDateTime);
            Player invalidPlayer = randomPlayer;

            var invalidPlayerException =
                new InvalidPlayerException();

            invalidPlayerException.AddData(
                key: nameof(Player.CreatedDate),
                values: "Date is not recent");

            var expectedPlayerValidationException =
                new PlayerValidationException(invalidPlayerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(invalidPlayer);

            PlayerValidationException actualPlayerValidationException =
                await Assert.ThrowsAsync<PlayerValidationException>(() =>
                    addPlayerTask.AsTask());

            // then
            actualPlayerValidationException.Should()
                .BeEquivalentTo(expectedPlayerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}