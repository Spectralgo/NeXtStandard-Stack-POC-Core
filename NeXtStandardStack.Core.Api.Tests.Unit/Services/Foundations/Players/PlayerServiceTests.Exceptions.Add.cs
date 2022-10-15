using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NeXtStandardStack.Core.Api.Models.Players;
using NeXtStandardStack.Core.Api.Models.Players.Exceptions;
using Xunit;

namespace NeXtStandardStack.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Player somePlayer = CreateRandomPlayer();
            SqlException sqlException = GetSqlException();

            var failedPlayerStorageException =
                new FailedPlayerStorageException(sqlException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedPlayerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(somePlayer);

            PlayerDependencyException actualPlayerDependencyException =
                await Assert.ThrowsAsync<PlayerDependencyException>(
                    addPlayerTask.AsTask);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfPlayerAlreadyExsitsAndLogItAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            Player alreadyExistsPlayer = randomPlayer;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsPlayerException =
                new AlreadyExistsPlayerException(duplicateKeyException);

            var expectedPlayerDependencyValidationException =
                new PlayerDependencyValidationException(alreadyExistsPlayerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Player> addPlayerTask =
                this.playerService.AddPlayerAsync(alreadyExistsPlayer);

            // then
            PlayerDependencyValidationException actualPlayerDependencyValidationException =
                await Assert.ThrowsAsync<PlayerDependencyValidationException>(
                    addPlayerTask.AsTask);

            actualPlayerDependencyValidationException.Should()
                .BeEquivalentTo(expectedPlayerDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPlayerAsync(It.IsAny<Player>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}