using System;
using NeXtStandardStack.Core.Api.Models.Players;
using NeXtStandardStack.Core.Api.Models.Players.Exceptions;

namespace NeXtStandardStack.Core.Api.Services.Foundations.Players
{
    public partial class PlayerService
    {
        private void ValidatePlayerOnAdd(Player player)
        {
            ValidatePlayerIsNotNull(player);

            Validate(
                (Rule: IsInvalid(player.Id), Parameter: nameof(Player.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(player.CreatedDate), Parameter: nameof(Player.CreatedDate)),
                (Rule: IsInvalid(player.CreatedByUserId), Parameter: nameof(Player.CreatedByUserId)),
                (Rule: IsInvalid(player.UpdatedDate), Parameter: nameof(Player.UpdatedDate)),
                (Rule: IsInvalid(player.UpdatedByUserId), Parameter: nameof(Player.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: player.UpdatedDate,
                    secondDate: player.CreatedDate,
                    secondDateName: nameof(Player.CreatedDate)),
                Parameter: nameof(Player.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: player.UpdatedByUserId,
                    secondId: player.CreatedByUserId,
                    secondIdName: nameof(Player.CreatedByUserId)),
                Parameter: nameof(Player.UpdatedByUserId)));
        }

        private static void ValidatePlayerIsNotNull(Player player)
        {
            if (player is null)
            {
                throw new NullPlayerException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPlayerException = new InvalidPlayerException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPlayerException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPlayerException.ThrowIfContainsErrors();
        }
    }
}