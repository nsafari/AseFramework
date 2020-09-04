using System;

namespace AseFramework.Messaging.Common.Wrapper
{
    public class InternalDateTimeOffset
    {
        public InternalDateTimeOffset(DateTimeOffset dateTimeOffset)
        {
            this.dateTimeOffset = dateTimeOffset;
        }

        public DateTimeOffset dateTimeOffset { get; set; }

    }
}