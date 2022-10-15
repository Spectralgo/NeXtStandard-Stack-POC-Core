using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class PlayerValidationException : Xeption
    {
        public PlayerValidationException(Xeption innerException)
            : base(message: "Player validation errors occurred, please try again.",
                  innerException)
        { }
    }
}