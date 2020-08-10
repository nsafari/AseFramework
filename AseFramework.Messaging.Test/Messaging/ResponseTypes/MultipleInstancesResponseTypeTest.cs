using System;
using System.Collections.Generic;
using System.Linq;
using Ase.Messaging.Messaging.ResponseTypes;
using Xunit;

namespace Ase.Messaging.Test.Messaging.ResponseTypes
{
    /// <summary>
    /// Test all possible permutations of Query Handler return types through the {@link MultipleInstancesResponseType}. To
    /// that end, leveraging the  {@link AbstractResponseTypeTest} to cover all usual suspects between the different
    /// {@link ResponseType} implementations.
    /// </summary>
    public class MultipleInstancesResponseTypeTest : AbstractResponseTypeTest<List<QueryResponse>>
    {
        public MultipleInstancesResponseTypeTest() : base(
            new MultipleInstancesResponseType<QueryResponse>(typeof(QueryResponse)))
        {
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsTheSame()
        {
            TestMatches("SomeQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsSubTypeOfProvidedType()
        {
            TestMatches("SomeSubTypedQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsSuperTypeOfProvidedType()
        {
            TestMatches("SomeSuperTypedQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGeneric()
        {
            TestMatches("SomeUnboundedGenericQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsBoundedGenericOfProvidedType()
        {
            TestMatches("SomeBoundedGenericQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMultiBoundedGenericOfProvidedType()
        {
            TestMatches("SomeMultiBoundedGenericQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericOfOtherType()
        {
            TestMatches("SomeNonMatchingBoundedGenericQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsArrayOfProvidedType()
        {
            TestMatches("SomeArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsArrayWithSubTypeOfProvidedType()
        {
            TestMatches("SomeSubTypedArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsArrayWithSuperTypeOfProvidedType()
        {
            TestMatches("SomeSuperTypedArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericArray()
        {
            TestMatches("SomeUnboundedGenericArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsBoundedGenericArrayOfProvidedType()
        {
            TestMatches("SomeBoundedGenericArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiBoundedGenericArrayOfProvidedType()
        {
            TestMatches("SomeMultiBoundedGenericArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericArrayOfOtherType()
        {
            TestMatches("SomeNonMatchingBoundedGenericArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsListOfProvidedType()
        {
            TestMatches("SomeListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsSubListOfProvidedType()
        {
            TestMatches("SomeSubListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsSuperListOfProvidedType()
        {
            TestMatches("SomeSuperListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsBoundedGenericListOfProvidedType()
        {
            TestMatches("SomeBoundedGenericListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericList()
        {
            TestMatches("SomeUnboundedGenericListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiBoundedGenericListOfProvidedType()
        {
            TestMatches("SomeMultiBoundedGenericListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericListOfOtherType()
        {
            TestMatches("SomeNonMatchingBoundedGenericListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedWildcardList()
        {
            TestMatches("SomeUnboundedWildcardListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsLowerBoundedGenericList()
        {
            TestMatches("SomeLowerBoundedWildcardListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsUpperBoundedWildcardListOfProvidedType()
        {
            TestMatches("SomeUpperBoundedWildcardListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsWildcardListOfOtherType()
        {
            TestMatches("SomeNonMatchingUpperBoundedWildcardQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericUpperBoundedWildcardList()
        {
            TestMatches("SomeUnboundedGenericUpperBoundedWildcardListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsGenericUpperBoundedWildcardListOfProvidedType()
        {
            TestMatches("SomeGenericUpperBoundedWildcardListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiGenericUpperBoundedWildcardListOfProvidedType()
        {
            TestMatches("SomeMultiGenericUpperBoundedWildcardListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsListImplementationOfProvidedType()
        {
            TestMatches("SomeListImplementationQuery", Matches);
        }

        /*
         This dummy function (QueryResponseList SomeListImplementationQuery) and dummy class (QueryResponseList) are
         contained in this test class instead of the AbstractResponseTypeTest class, because the functionality to derive
         whether a response type has a direct super type which we service (an Iterable in this case), checks if the
         enclosing classes contain unresolved generic types. It does this to check whether the type has raw types or not.
         Since the AbstractResponseTypeTest has a generic type R for test implementations, a check by that functionality for
         AbstractResponseTypeTest.QueryResponseList results in the state that it thinks it's unresolved
         (whilst in fact it is). This is however such a slim scenario, that I decided to put the dummy class and test
         function in the actual test class itself instead of in the abstract test class.
         */
        public static QueryResponseList SomeListImplementationQuery()
        {
            return new QueryResponseList();
        }


        public class QueryResponseList: List<QueryResponse> {
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedListImplementationOfProvidedType()
        {
            TestMatches("SomeUnboundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsBoundedListImplementationOfProvidedType()
        {
            TestMatches("SomeBoundedListImplementationQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMultiUnboundedListImplementationOfProvidedType()
        {
            TestMatches("SomeMultiUnboundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiBoundedListImplementationOfProvidedType()
        {
            TestMatches("SomeMultiBoundedListImplementationQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsSetOfProvidedType()
        {
            TestMatches("SomeSetQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsStreamOfProvidedType()
        {
            TestMatches("SomeStreamQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMapOfProvidedType()
        {
            TestMatches("SomeMapQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsFutureOfProvidedType()
        {
            TestMatches("SomeFutureQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsListOfFutureOfProvidedType()
        {
            TestMatches("SomeFutureListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsOptionalOfProvidedType()
        {
            TestMatches("SomeOptionalQueryResponse", DoesNotMatch);
        }

        
        [Fact]
        void TestConvertThrowsExceptionForSingleInstanceResponse()
        {
            Assert.Throws<ArgumentException>(() => TestSubject.Convert(new QueryResponse()));
        }

        [Fact]
        void TestConvertReturnsListOnResponseOfArrayType()
        {
            QueryResponse[] testResponse = new QueryResponse[] {new QueryResponse()};

            List<QueryResponse> result = TestSubject.Convert(testResponse);

            Assert.Equal(testResponse.Length, result.Count);
            Assert.Equal(testResponse[0], result[0]);
        }

        [Fact]
        void TestConvertReturnsListOnResponseOfSubTypedArrayType()
        {
            SubTypedQueryResponse[] testResponse = new SubTypedQueryResponse[] {new SubTypedQueryResponse()};

            List<QueryResponse> result = TestSubject.Convert(testResponse);

            Assert.Equal(testResponse.Length, result.Count);
            Assert.Equal(testResponse[0], result[0]);
        }

        

        [Fact]
        void TestConvertThrowsExceptionForResponseOfDifferentArrayType()
        {
            IQueryResponseInterface[] testResponse = {
                new QueryResponseFromInterface()
                {
                }
            };

            Assert.Throws<ArgumentException>(() => TestSubject.Convert(testResponse));
        }

        [Fact]
        void TestConvertReturnsListOnResponseOfListType()
        {
            List<QueryResponse> testResponse = new List<QueryResponse>();
            testResponse.Add(new QueryResponse());

            List<QueryResponse> result = TestSubject.Convert(testResponse);

            Assert.Equal(testResponse.Count, result.Count);
            Assert.Equal(testResponse[0], result[0]);
        }

        [Fact]
        void TestConvertReturnsListOnResponseOfSubTypedListType()
        {
            List<SubTypedQueryResponse> testResponse = new List<SubTypedQueryResponse>();
            testResponse.Add(new SubTypedQueryResponse());

            List<QueryResponse> result = TestSubject.Convert(testResponse);

            Assert.Equal(testResponse.Count, result.Count);
            Assert.Equal(testResponse[0], result[0]);
        }

        [Fact]
        void TestConvertReturnsListOnResponseOfSetType()
        {
            HashSet<QueryResponse> testResponse = new HashSet<QueryResponse>();
            testResponse.Add(new QueryResponse());

            List<QueryResponse> result = TestSubject.Convert(testResponse);

            Assert.Equal(testResponse.Count, result.Count);
            Assert.Equal(testResponse.First(), result[0]);
        }

        

        [Fact]
        void TestConvertThrowsExceptionForResponseOfDifferentListType()
        {
            List<QueryResponseFromInterface> testResponse = new List<QueryResponseFromInterface>();
            testResponse.Add(new QueryResponseFromInterface()
            {
            });

            Assert.Throws<ArgumentException>(() => TestSubject.Convert(testResponse));
        }

        [Fact]
        void TestConvertReturnsEmptyListForResponseOfDifferentListTypeIfTheListIsEmpty()
        {
            List<IQueryResponseInterface> testResponse = new List<IQueryResponseInterface>();

            List<QueryResponse> result = TestSubject.Convert(testResponse);

            Assert.True(!result.Any());
        }
        
    }
    
    
}