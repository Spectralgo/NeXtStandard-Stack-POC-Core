using System;

namespace NeXtStandardStack.Core.Api.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTimeOffset();
    }
}
