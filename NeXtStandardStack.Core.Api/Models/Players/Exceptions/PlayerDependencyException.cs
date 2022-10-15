using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class PlayerDependencyException : Xeption
    {
        public PlayerDependencyException(Xeption innerException) :
            base(message: "Player dependency error occurred, contact support.", innerException)
        { }
    }
}