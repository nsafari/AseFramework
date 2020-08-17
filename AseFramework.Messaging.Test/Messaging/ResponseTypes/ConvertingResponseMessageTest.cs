using System;
using System.Collections.Generic;
using System.Linq;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.ResponseTypes;
using Ase.Messaging.QueryHandling;
using Xunit;

namespace Ase.Messaging.Test.Messaging.ResponseTypes
{
    public class ConvertingResponseMessageTest
    {
        [Fact]
        void TestPayloadIsConvertedToExpectedType()
        {
            IQueryResponseMessage<string[]> msg =
                new GenericQueryResponseMessage<string[]>(new[] {"Some string result"})
                    .WithMetaData(MetaData.With("test", "value"));
            IQueryResponseMessage<List<string>> wrapped = new ConvertingResponseMessage<List<string>, string[]>(
                Ase.Messaging.Messaging.ResponseTypes.ResponseTypes.MultipleInstancesOf<string>(typeof(string)),
                msg);

            Assert.Equal(typeof(List<>), wrapped.GetPayloadType());
            Assert.Equal(new List<string>() {"Some string result"}, wrapped.GetPayload());
            Assert.Equal("value", wrapped.GetMetaData()["test"]);
        }
    }
}