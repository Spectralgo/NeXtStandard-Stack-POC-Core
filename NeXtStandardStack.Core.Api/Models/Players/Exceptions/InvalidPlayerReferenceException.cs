using System;
using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class InvalidPlayerReferenceException : Xeption
    {
        public InvalidPlayerReferenceException(Exception innerException)
            : base(message: "Invalid player reference error occurred.", innerException) { }
    }
}