using System;
using System.Collections.Immutable;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging;
using Xunit;

namespace Ase.Messaging.Test.EventHandling
{
    public class GenericDomainEventMessageTest
    {
        
        [Fact]
        private void TestConstructor()
        {
            object payload = new object();
            long seqNo = 0;
            string id = Guid.NewGuid().ToString();
            GenericDomainEventMessage<object> message1 = new GenericDomainEventMessage<object>("type", id, seqNo, payload);
            IImmutableDictionary<string, object> metaDataMap = ImmutableDictionary<string, object>.Empty.Add("key", "value");
            MetaData metaData = MetaData.From(metaDataMap);
            GenericDomainEventMessage<object> message2 =
                new GenericDomainEventMessage<object>("type", id, seqNo, payload, metaData);
            GenericDomainEventMessage<object> message3 =
                new GenericDomainEventMessage<object>("type", id, seqNo, payload, metaDataMap);

            Assert.Same(id, message1.GetAggregateIdentifier());
            Assert.Equal(seqNo, message1.GetSequenceNumber());
            Assert.Same(MetaData.EmptyInstance, message1.GetMetaData());
            Assert.Equal(typeof(object), message1.GetPayload()!.GetType());
            Assert.Equal(typeof(object), message1.GetPayloadType());

            Assert.Same(id, message2.GetAggregateIdentifier());
            Assert.Equal(seqNo, message2.GetSequenceNumber());
            Assert.Same(metaData, message2.GetMetaData());
            Assert.Equal(typeof(object), message2.GetPayload()!.GetType());
            Assert.Equal(typeof(object), message2.GetPayloadType());

            Assert.Same(id, message3.GetAggregateIdentifier());
            Assert.Equal(seqNo, message3.GetSequenceNumber());
            Assert.NotSame(metaDataMap, message3.GetMetaData());
            Assert.Equal(metaDataMap, message3.GetMetaData());
            Assert.Equal(typeof(object), message3.GetPayload()!.GetType());
            Assert.Equal(typeof(object), message3.GetPayloadType());

            Assert.NotEqual(message1.GetIdentifier(), message2.GetIdentifier());
            Assert.NotEqual(message1.GetIdentifier(), message3.GetIdentifier());
            Assert.NotEqual(message2.GetIdentifier(), message3.GetIdentifier());
        }

        [Fact]
        private void TestWithMetaData()
        {
            object payload = new object();
            long seqNo = 0;
            string id = Guid.NewGuid().ToString();
            IImmutableDictionary<string, object> metaDataMap = ImmutableDictionary<string, object>.Empty.Add("key", "value");
            MetaData metaData = MetaData.From(metaDataMap);
            GenericDomainEventMessage<object> message =
                new GenericDomainEventMessage<object>("type", id, seqNo, payload, metaData);
            GenericDomainEventMessage<object> message1 = (GenericDomainEventMessage<object>) message.WithMetaData(MetaData.EmptyInstance);
            GenericDomainEventMessage<object> message2 = (GenericDomainEventMessage<object>) message.WithMetaData(
                MetaData.From(ImmutableDictionary<string, object>.Empty.Add("key", "otherValue")));

            Assert.Empty(message1.GetMetaData());
            Assert.Single(message2.GetMetaData());
        }

        [Fact]
        private void TestAndMetaData()
        {
            object payload = new object();
            long seqNo = 0;
            string id = Guid.NewGuid().ToString();
            IImmutableDictionary<string, object> metaDataMap = ImmutableDictionary<string, object>.Empty.Add("key", "value");
            MetaData metaData = MetaData.From(metaDataMap);
            GenericDomainEventMessage<object> message =
                new GenericDomainEventMessage<object>("type", id, seqNo, payload, metaData);
            GenericDomainEventMessage<object> message1 = (GenericDomainEventMessage<object>) message.AndMetaData(MetaData.EmptyInstance);
            GenericDomainEventMessage<object> message2 = (GenericDomainEventMessage<object>) message.AndMetaData(
                MetaData.From(ImmutableDictionary<string, object>.Empty.Add("key", "otherValue")));

            Assert.Single(message1.GetMetaData());
            Assert.Equal("value", message1.GetMetaData()["key"]);
            Assert.Single(message2.GetMetaData());
            Assert.Equal("otherValue", message2.GetMetaData()["key"]);
        }

        [Fact]
        private void TestToString()
        {
            string actual = new GenericDomainEventMessage<object>("AggregateType", "id1", 1, "MyPayload")
                .AndMetaData(MetaData.With("key", "value").And("key2", 13)).ToString()!;
            Assert.True(actual.StartsWith("GenericDomainEventMessage{payload={MyPayload}, metadata={"),
                "Wrong output: " + actual);
            Assert.True(actual.Contains("'key'->'value'"), "Wrong output: " + actual);
            Assert.True(actual.Contains("'key2'->'13'"), "Wrong output: " + actual);
            Assert.True(actual.Contains("', timestamp='"), "Wrong output: " + actual);
            Assert.True(actual.Contains("', aggregateIdentifier='id1'"), "Wrong output: " + actual);
            Assert.True(actual.Contains("', aggregateType='AggregateType'"), "Wrong output: " + actual);
            Assert.True(actual.Contains("', sequenceNumber=1"), "Wrong output: " + actual);
            Assert.True(actual.EndsWith("}"), "Wrong output: " + actual);
        }
        
    }
}