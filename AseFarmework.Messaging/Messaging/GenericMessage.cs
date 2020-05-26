namespace Ase.Messaging.Messaging
{
    public class GenericMessage<T>: AbstractMessage<T>
    {
        public GenericMessage(string identifier) : base(identifier)
        {
        }

        public override MetaData GetMetaData()
        {
            throw new System.NotImplementedException();
        }

        public override T GetPayload()
        {
            throw new System.NotImplementedException();
        }

        protected override IMessage<T> WithMetaData(MetaData metaData)
        {
            throw new System.NotImplementedException();
        }
    }
}