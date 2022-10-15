using System;
using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class PlayerServiceException : Xeption
    {
        public PlayerServiceException(Exception innerException)
            : base(message: "Player service error occurred, contact support.", innerException)
        { }
    }
}