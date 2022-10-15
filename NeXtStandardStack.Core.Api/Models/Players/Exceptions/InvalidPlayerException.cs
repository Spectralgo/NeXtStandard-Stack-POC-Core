using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class InvalidPlayerException : Xeption
    {
        public InvalidPlayerException()
            : base(message: "Invalid player. Please correct the errors and try again.")
        { }
    }
}