using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using NeXtStandardStack.Core.Api.Models.Players;
using NeXtStandardStack.Core.Api.Models.Players.Exceptions;
using Xunit;

namespace NeXtStandardStack.Core.Api.Tests.Unit.Services.Foundations.Players
{
    public partial class PlayerServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            SqlException sqlException = GetSqlException();

            var failedPlayerStorageException =
                new FailedPlayerStorageException(sqlException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedPlayerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(randomPlayer);

            PlayerDependencyException actualPlayerDependencyException =
                await Assert.ThrowsAsync<PlayerDependencyException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(randomPlayer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(randomPlayer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Player somePlayer = CreateRandomPlayer();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidPlayerReferenceException =
                new InvalidPlayerReferenceException(foreignKeyConstraintConflictException);

            PlayerDependencyValidationException expectedPlayerDependencyValidationException =
                new PlayerDependencyValidationException(invalidPlayerReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(somePlayer);

            PlayerDependencyValidationException actualPlayerDependencyValidationException =
                await Assert.ThrowsAsync<PlayerDependencyValidationException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerDependencyValidationException.Should()
                .BeEquivalentTo(expectedPlayerDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(somePlayer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPlayerDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(somePlayer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            var databaseUpdateException = new DbUpdateException();

            var failedPlayerStorageException =
                new FailedPlayerStorageException(databaseUpdateException);

            var expectedPlayerDependencyException =
                new PlayerDependencyException(failedPlayerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(randomPlayer);

            PlayerDependencyException actualPlayerDependencyException =
                await Assert.ThrowsAsync<PlayerDependencyException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerDependencyException.Should()
                .BeEquivalentTo(expectedPlayerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(randomPlayer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(randomPlayer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPlayerException =
                new LockedPlayerException(databaseUpdateConcurrencyException);

            var expectedPlayerDependencyValidationException =
                new PlayerDependencyValidationException(lockedPlayerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(randomPlayer);

            PlayerDependencyValidationException actualPlayerDependencyValidationException =
                await Assert.ThrowsAsync<PlayerDependencyValidationException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerDependencyValidationException.Should()
                .BeEquivalentTo(expectedPlayerDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(randomPlayer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(randomPlayer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Player randomPlayer = CreateRandomPlayer();
            var serviceException = new Exception();

            var failedPlayerServiceException =
                new FailedPlayerServiceException(serviceException);

            var expectedPlayerServiceException =
                new PlayerServiceException(failedPlayerServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Player> modifyPlayerTask =
                this.playerService.ModifyPlayerAsync(randomPlayer);

            PlayerServiceException actualPlayerServiceException =
                await Assert.ThrowsAsync<PlayerServiceException>(
                    modifyPlayerTask.AsTask);

            // then
            actualPlayerServiceException.Should()
                .BeEquivalentTo(expectedPlayerServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPlayerByIdAsync(randomPlayer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPlayerServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePlayerAsync(randomPlayer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}