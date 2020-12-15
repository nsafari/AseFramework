using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ase.Messaging.Common;
using NHibernate.Engine;

namespace Ase.Messaging.EventHandling
{
    public class GapAwareTrackingToken : ITrackingToken
    {
        private readonly long _index;
        private readonly SortedSet<long> _gaps;
        private readonly long _gapTruncationIndex;

        public static GapAwareTrackingToken NewInstance(long index, Collection<long> gaps)
        {
            return new GapAwareTrackingToken(index, gaps);
        }

        public GapAwareTrackingToken(long index, Collection<long> gaps) : this(index, createSortedSetOf(gaps, index), 0)
        {
        }
        
        private GapAwareTrackingToken(long index, SortedSet<long> gaps, long gapTruncationIndex) {
            _index = index;
            _gaps = gaps;
            _gapTruncationIndex = gapTruncationIndex;
        }
        
        protected static SortedSet<long> CreateSortedSetOf(Collection<long> gaps, long index) {
            if (gaps.Count == 0) {
                return new SortedSet<long>();
            }
            SortedList gapSet = SortedList.Synchronized(new SortedList(gaps));
            Assert.isTrue(gapSet.last() < index,
                () -> String.format("Gap indices [%s] should all be smaller than head index [%d]", gaps, index));
            return gapSet;
        }

    }
}