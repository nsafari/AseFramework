using System.Collections.Immutable;
using System.Text;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.ResponseTypes;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// Generic implementation of the QueryMessage. Unless explicitly provided, it assumes the {@code queryName} of the
    /// message is the fully qualified class name of the message's payload.
    /// </summary>
    /// <typeparam name="T">The type of payload expressing the query in this message</typeparam>
    /// <typeparam name="TR">The type of response expected from this query</typeparam>
    public class GenericQueryMessage<T, TR> : MessageDecorator<T>, IQueryMessage<T, TR> 
        where T : class where TR : class
    {
        private readonly string _queryName;
        private readonly IResponseType<TR> _responseType;


        /// <summary>
        /// Initializes the message with the given {@code payload} and expected {@code responseType}. The query name is
        /// set to the fully qualified class name of the {@code payload}.
        /// </summary>
        /// <param name="payload">The payload expressing the query</param>
        /// <param name="responseType">The expected response type of type</param>
        public GenericQueryMessage(T payload, IResponseType<TR> responseType)
            : this(payload, payload.GetType().Name, responseType) {
            
        }

        
        /// <summary>
        /// Initializes the message with the given {@code payload}, {@code queryName} and expected {@code responseType}.
        /// </summary>
        /// <param name="payload">The payload expressing the query</param>
        /// <param name="queryName">The name identifying the query to execute</param>
        /// <param name="responseType">The expected response type of type</param>
        public GenericQueryMessage(T payload, string queryName, IResponseType<TR> responseType)
            : this(new GenericMessage<T>(payload, MetaData.EmptyInstance), queryName, responseType) {
            
        }

        /// <summary>
        /// Initialize the Query Message, using given {@code delegate} as the carrier of payload and metadata and given
        /// {@code queryName} and expecting the given {@code responseType}.
        /// </summary>
        /// <param name="delegate">The message containing the payload and meta data for this message</param>
        /// <param name="queryName">The name identifying the query to execute</param>
        /// <param name="responseType">The expected response type of type {@link ResponseType}</param>
        public GenericQueryMessage(IMessage<T> @delegate, string queryName, IResponseType<TR> responseType)
            :base(@delegate) {
            _responseType = responseType;
            _queryName = queryName;
        }
        
        public string GetQueryName() {
            return _queryName;
        }

        public IResponseType<TR> GetResponseType() {
            return _responseType;
        }

        public IQueryMessage<T, TR> WithMetaData(IImmutableDictionary<string, object> metaData) {
            return new GenericQueryMessage<T, TR>(Delegate().WithMetaData(metaData), _queryName, _responseType);
        }

        public IQueryMessage<T, TR> AndMetaData(IImmutableDictionary<string, object> metaData) {
            return new GenericQueryMessage<T, TR>(Delegate().AndMetaData(metaData), _queryName, _responseType);
        }

        protected override void DescribeTo(StringBuilder stringBuilder) {
            base.DescribeTo(stringBuilder);
            stringBuilder.Append(", queryName='")
                .Append(GetQueryName())
                .Append('\'')
                .Append(", expectedResponseType='")
                .Append(GetResponseType())
                .Append('\'');
        }
 
        protected override string DescribeType() {
            return "GenericQueryMessage";
        }
    }
}