using System;
using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class NotFoundPlayerException : Xeption
    {
        public NotFoundPlayerException(Guid playerId)
            : base(message: $"Couldn't find player with playerId: {playerId}.")
        { }
    }
}