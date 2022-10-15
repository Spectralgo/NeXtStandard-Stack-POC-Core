using System;
using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class FailedPlayerServiceException : Xeption
    {
        public FailedPlayerServiceException(Exception innerException)
            : base(message: "Failed player service occurred, please contact support", innerException)
        { }
    }
}