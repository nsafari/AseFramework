using System;

namespace Ase.Messaging.Common.Wrapper
{
    public class InternalDateTimeOffset
    {
        public InternalDateTimeOffset(DateTimeOffset dateTimeOffset)
        {
            DateTimeOffset = dateTimeOffset;
        }

        public DateTimeOffset DateTimeOffset { get; set; }

    }
}