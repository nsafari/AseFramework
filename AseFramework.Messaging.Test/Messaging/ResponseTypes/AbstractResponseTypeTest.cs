using System;
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
        
        public Object SomeSuperTypedQuery() {
            return new Object();
        }

    }

    public class QueryResponse
    {
    }


    public class SubTypedQueryResponse: QueryResponse {
    }
    
}