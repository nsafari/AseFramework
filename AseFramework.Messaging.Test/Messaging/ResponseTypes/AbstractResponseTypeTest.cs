using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ase.Messaging.Messaging.ResponseTypes;
using Assert = Xunit.Assert;

namespace Ase.Messaging.Test.Messaging.ResponseTypes
{
    /// <summary>
    /// Helper test implementation of {@link ResponseType} tests.
    ///
    /// <typeparam name="R">a generic for the expected response type of the {@link ResponseType} test subject</typeparam>
    /// </summary>
    public class AbstractResponseTypeTest<R>
    {
        protected static readonly bool Matches = true;
        protected static readonly bool DoesNotMatch = false;

        protected readonly IResponseType<R> TestSubject;

        protected AbstractResponseTypeTest(IResponseType<R> testSubject)
        {
            TestSubject = testSubject;
        }

        /// <summary>
        /// Helper function to make testing of the
        /// {@link ResponseType#matches(Type)} function easier.
        /// Takes a {@code methodNameToTest} which it uses to pull a {@link java.lang.reflect.Method} from this abstract
        /// class. There after it will pull the return {@link java.lang.reflect.Type} from that method, which it will
        /// use as input for the test subject's match function.
        /// </summary>
        /// <param name="methodNameToTest">a {@link java.lang.String} representing the function you want to extract a
        /// return type from</param>
        /// <param name="expectedResult">a {@link java.lang.Boolean} which is the expected result of the matches
        /// call</param>
        protected void TestMatches(string methodNameToTest, bool expectedResult)
        {
            var methodInfo = this.GetType().GetMethod(methodNameToTest);
            if (methodInfo == null)
            {
                throw new MissingMethodException(methodNameToTest);
            }

            var methodInfoReturnType = methodInfo.ReturnType;
            Assert.Equal(expectedResult, TestSubject.Matches(methodInfoReturnType));
        }

        public QueryResponse SomeQuery()
        {
            return new QueryResponse();
        }

        public SubTypedQueryResponse SomeSubTypedQuery()
        {
            return new SubTypedQueryResponse();
        }

        public Object SomeSuperTypedQuery()
        {
            return new Object();
        }

        public E SomeUnboundedGenericQuery<E>()
        {
            return default;
        }

        public E SomeBoundedGenericQuery<E>()
            where E : QueryResponse
        {
            return new SubTypedQueryResponse() as E;
        }

        public E SomeMultiBoundedGenericQuery<E>()
            where E : SubTypedQueryResponse, IQueryResponseInterface
        {
            return new ComplexTypedQueryResponse() as E;
        }

        public E SomeNonMatchingBoundedGenericQuery<E>()
            where E : class, IQueryResponseInterface
        {
            return new { } as E;
        }

        public QueryResponse[] SomeArrayQuery()
        {
            return new QueryResponse[] { };
        }

        public SubTypedQueryResponse[] SomeSubTypedArrayQuery()
        {
            return new SubTypedQueryResponse[] { };
        }


        public Object[] SomeSuperTypedArrayQuery()
        {
            return new Object[] { };
        }

        public E[] SomeUnboundedGenericArrayQuery<E>()
        {
            return new SubTypedQueryResponse[] { } as E[];
        }

        public E[] SomeBoundedGenericArrayQuery<E>()
            where E : QueryResponse
        {
            return new SubTypedQueryResponse[] { } as E[];
        }

        public E[] SomeMultiBoundedGenericArrayQuery<E>()
            where E : SubTypedQueryResponse, IQueryResponseInterface
        {
            return new ComplexTypedQueryResponse[] { } as E[];
        }

        public E[] SomeNonMatchingBoundedGenericArrayQuery<E>()
            where E : IQueryResponseInterface
        {
            return new SubTypedQueryResponse[] { } as E[];
        }


        public List<QueryResponse> SomeListQuery()
        {
            return new List<QueryResponse>();
        }


        public List<SubTypedQueryResponse> SomeSubListQuery()
        {
            return new List<SubTypedQueryResponse>();
        }


        public List<object> SomeSuperListQuery()
        {
            return new List<object>();
        }


        public List<E> SomeBoundedGenericListQuery<E>()
            where E : QueryResponse
        {
            return new List<E>();
        }


        public List<E> SomeUnboundedGenericListQuery<E>()
        {
            return new List<E>();
        }


        public List<E> SomeMultiBoundedGenericListQuery<E>()
            where E : SubTypedQueryResponse, IQueryResponseInterface
        {
            return new List<E>();
        }


        public List<E> SomeNonMatchingBoundedGenericListQuery<E>()
            where E : IQueryResponseInterface
        {
            return new List<E>();
        }

        public UnboundQueryResponseList<E> SomeUnboundedListImplementationQuery<E>()
        {
            return new UnboundQueryResponseList<E>();
        }


        public BoundQueryResponseList<E> SomeBoundedListImplementationQuery<E>()
            where E : QueryResponse
        {
            return new BoundQueryResponseList<E>();
        }

        public MultiUnboundQueryResponseList<E, R> SomeMultiUnboundedListImplementationQuery<E, R>()
        {
            return new MultiUnboundQueryResponseList<E, R>();
        }

        public MultiBoundQueryResponseList<E, R> SomeMultiBoundedListImplementationQuery<E, R>()
            where E : QueryResponse
        {
            return new MultiBoundQueryResponseList<E, R>();
        }


        public ISet<QueryResponse> SomeSetQuery()
        {
            return new HashSet<QueryResponse>();
        }


        public IQueryable<QueryResponse> SomeStreamQuery()
        {
            return new List<QueryResponse>().AsQueryable();
        }


        public IDictionary<QueryResponse, QueryResponse> SomeMapQuery()
        {
            return new Dictionary<QueryResponse, QueryResponse>();
        }


        public Task<QueryResponse> SomeFutureQuery()
        {
            return Task.FromResult(new QueryResponse());
        }


        public Task<List<QueryResponse>> SomeFutureListQuery()
        {
            return Task.FromResult(new List<QueryResponse> {new QueryResponse()}.AsReadOnly().ToList());
        }


        public QueryResponse? SomeOptionalQueryResponse()
        {
            return new QueryResponse();
        }

        public List<T> SomeLowerBoundedWildcardListQuery<T>()
            where T : QueryResponse
        {
            return new List<T>();
        }

        public List<T> SomeUpperBoundedWildcardListQuery<T>()
            where T : SubTypedQueryResponse
        {
            return new List<T>();
        }

        public List<T> SomeNonMatchingUpperBoundedWildcardQuery<T>()
            where T : IQueryResponseInterface
        {
            return new List<T>();
        }

        public List<T> SomeUnboundedGenericUpperBoundedWildcardListQuery<T>()
        {
            return new List<T>();
        }

        public List<T> SomeGenericUpperBoundedWildcardListQuery<T>()
            where T : SubTypedQueryResponse
        {
            return new List<T>();
        }

        public List<T> SomeMultiGenericUpperBoundedWildcardListQuery<T>()
            where T : SubTypedQueryResponse, IQueryResponseInterface
        {
            return new List<T>();
        }

        public List<T> SomeUnboundedWildcardListQuery<T>() {
            return new List<T>();
        }

    }

    public class QueryResponse
    {
    }


    public class SubTypedQueryResponse : QueryResponse
    {
    }

    public interface IQueryResponseInterface
    {
    }

    public class QueryResponseFromInterface : IQueryResponseInterface
    {
    }

    public class ComplexTypedQueryResponse : SubTypedQueryResponse, IQueryResponseInterface
    {
    }

    public class UnboundQueryResponseList<E> : List<E>
    {
    }

    public class BoundQueryResponseList<E> : List<E>
        where E : QueryResponse
    {
    }

    public class MultiUnboundQueryResponseList<E, R> : List<E>
    {
    }

    public class MultiBoundQueryResponseList<E, R> : List<E>
        where E : QueryResponse
    {
    }
}