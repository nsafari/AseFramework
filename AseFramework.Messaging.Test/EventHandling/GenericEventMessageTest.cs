using System;
using System.Collections.Immutable;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging;
using Xunit;

namespace Ase.Messaging.Test.EventHandling
{
    public class GenericEventMessageTest
    {
        [Fact]
        private void TestConstructor()
        {
            object payload = new object();
            GenericEventMessage<object> message1 = new GenericEventMessage<object>(payload);
            IImmutableDictionary<string, object> metaDataMap =
                ImmutableDictionary<string, object>.Empty.Add("key", "value");

            MetaData metaData = MetaData.From(metaDataMap);
            GenericEventMessage<object> message2 = new GenericEventMessage<object>(payload, metaData);
            GenericEventMessage<object> message3 = new GenericEventMessage<object>(payload, metaDataMap);

            Assert.Same(MetaData.EmptyInstance, message1.GetMetaData());
            Assert.Equal(typeof(object), message1.GetPayload()!.GetType());
            Assert.Equal(typeof(object), message1.GetPayloadType());

            Assert.Equal(metaData, message2.GetMetaData());
            Assert.Equal(typeof(object), message2.GetPayload()!.GetType());
            Assert.Equal(typeof(object), message2.GetPayloadType());

            Assert.NotSame(metaDataMap, message3.GetMetaData());
            Assert.Equal(metaDataMap, message3.GetMetaData());
            Assert.Equal(typeof(object), message3.GetPayload()!.GetType());
            Assert.Equal(typeof(object), message3.GetPayloadType());

            Assert.False(message1.GetIdentifier().Equals(message2.GetIdentifier()));
            Assert.False(message1.GetIdentifier().Equals(message3.GetIdentifier()));
            Assert.False(message2.GetIdentifier().Equals(message3.GetIdentifier()));
        }

        [Fact]
        private void TestWithMetaData()
        {
            object payload = new object();
            IImmutableDictionary<string, object> metaDataMap =
                ImmutableDictionary<string, object>.Empty.Add("key", "value");
            MetaData metaData = MetaData.From(metaDataMap);
            GenericEventMessage<object> message = new GenericEventMessage<object>(payload, metaData);
            GenericEventMessage<object> message1 = (GenericEventMessage<object>) message.WithMetaData(MetaData.EmptyInstance);
            GenericEventMessage<object> message2 = (GenericEventMessage<object>) message.WithMetaData(
                MetaData.From(ImmutableDictionary<string, object>.Empty.Add("key", "otherValue")));

            Assert.Empty(message1.GetMetaData());
            Assert.Single(message2.GetMetaData());
        }


        [Fact]

        private void TestAndMetaData()
        {
            object payload = new object();
            ImmutableDictionary<string, object> metaDataMap = ImmutableDictionary<string, object>.Empty.Add("key", "value");
            MetaData metaData = MetaData.From(metaDataMap);;
            GenericEventMessage<object> message = new GenericEventMessage<object>(payload, metaData);
            GenericEventMessage<object> message1 = (GenericEventMessage<object>) message.AndMetaData(MetaData.EmptyInstance);
            GenericEventMessage<object> message2 = (GenericEventMessage<object>) message.AndMetaData(
                ImmutableDictionary<string, object>.Empty.Add("key", "otherValue"));

            Assert.Single(message1.GetMetaData());
            Assert.Equal("value", message1.GetMetaData()["key"]);
            Assert.Single(message2.GetMetaData());
            Assert.Equal("otherValue", message2.GetMetaData()["key"]);
        }

        // [Fact]
        // private void TestTimestampInEventMessageIsAlwaysSerialized() {
        //     final ByteArrayOutputStream baos = new ByteArrayOutputStream();
        //     objectOutputStream oos = new objectOutputStream(baos);
        //     GenericEventMessage<string> testSubject =
        //         new GenericEventMessage<>(new GenericMessage<>("payload", Collections.singletonMap("key", "value")),
        //             Instant::now);
        //     oos.writeobject(testSubject);
        //     objectInputStream ois = new objectInputStream(new ByteArrayInputStream(baos.toByteArray()));
        //     object read = ois.readobject();
        //
        //     Assert.Equal(GenericEventMessage.class, read.getClass());
        //     assertNotNull(((GenericEventMessage < ? >) read).getTimestamp());
        // }

        [Fact]
        private void TestToString()
        {
            string actual = GenericEventMessage<string>.AsEventMessage<string>("MyPayload")
                .AndMetaData(MetaData.With("key", "value").And("key2", 13)).ToString()!;
            Assert.True(actual.StartsWith("MessageDecorator`1{payload={MyPayload}, metadata={"),
                "Wrong output: " + actual);
            Assert.True(actual.Contains("'key'->'value'"), "Wrong output: " + actual);
            Assert.True(actual.Contains("'key2'->'13'"), "Wrong output: " + actual);
            Assert.True(actual.Contains("', timestamp='"), "Wrong output: " + actual);
            Assert.True(actual.EndsWith("}"), "Wrong output: " + actual);
        }
    }
}