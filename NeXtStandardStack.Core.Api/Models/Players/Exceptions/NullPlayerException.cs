using Xeptions;

namespace NeXtStandardStack.Core.Api.Models.Players.Exceptions
{
    public class NullPlayerException : Xeption
    {
        public NullPlayerException()
            : base(message: "Player is null.")
        { }
    }
}