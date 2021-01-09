using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Ase.Messaging.Common;
using NHibernate.Engine;
using NHibernate.Util;

namespace Ase.Messaging.EventHandling
{
    public class GapAwareTrackingToken : ITrackingToken
    {
        private readonly long _index;
        private readonly ImmutableSortedSet<long> _gaps;
        private readonly long _gapTruncationIndex;

        public static GapAwareTrackingToken NewInstance(long index, Collection<long> gaps)
        {
            return new GapAwareTrackingToken(index, gaps);
        }

        public GapAwareTrackingToken(long index, Collection<long> gaps) : this(index, CreateSortedSetOf(gaps, index), 0)
        {
        }

        private GapAwareTrackingToken(long index, ImmutableSortedSet<long> gaps, long gapTruncationIndex)
        {
            _index = index;
            _gaps = gaps;
            _gapTruncationIndex = gapTruncationIndex;
        }

        protected static ImmutableSortedSet<long> CreateSortedSetOf(Collection<long> gaps, long index)
        {
            if (gaps.Count == 0)
            {
                return ImmutableSortedSet.Create<long>();
            }

            ImmutableSortedSet<long> gapSet = ImmutableSortedSet.CreateRange(gaps.AsEnumerable());
            Assert.IsTrue(gapSet.Max < index, () => throw new ArgumentException(
                $"Gap indices {gaps} should all be smaller than head index {index}"));
            return gapSet;
        }

        public GapAwareTrackingToken AdvanceTo(long index, int maxGapOffset)
        {
            long newIndex;
            ImmutableSortedSet<long> gaps = ImmutableSortedSet.Create(_gaps.ToArray());
            if (_gaps.Contains(index))
            {
                gaps = gaps.Remove(index);
                newIndex = _index;
            }
            else if (index > _index)
            {
                newIndex = index;
                var immutableBuilder = gaps.ToBuilder();
                foreach (var i in Enumerable.Range((int) (_index + 1L), (int)index))
                {
                    immutableBuilder.Add(i);
                }

                gaps = immutableBuilder.ToImmutable();
            }
            else
            {
                throw new ArgumentException(
                    $"The given index {index} should be larger than the token index {_index} or " +
                    $"be one of the token's gaps {gaps}");
            }

            long smalledAllowedGap = Math.Max(_gapTruncationIndex, newIndex - maxGapOffset);
            gaps = gaps.Where(x => x >= smalledAllowedGap).ToImmutableSortedSet();
            return new GapAwareTrackingToken(newIndex, gaps, smalledAllowedGap);
        }
        
        
    }
}