using System;
using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class FailedPlayerStorageException : Xeption
    {
        public FailedPlayerStorageException(Exception innerException)
            : base(message: "Failed player storage error occurred, contact support.", innerException)
        { }
    }
}