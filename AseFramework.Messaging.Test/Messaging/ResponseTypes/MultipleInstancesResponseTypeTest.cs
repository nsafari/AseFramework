using System;
using System.Collections.Generic;
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
            TestMatches("someMultiBoundedGenericQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericOfOtherType()
        {
            TestMatches("someNonMatchingBoundedGenericQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsArrayOfProvidedType()
        {
            TestMatches("someArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsArrayWithSubTypeOfProvidedType()
        {
            TestMatches("someSubTypedArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsArrayWithSuperTypeOfProvidedType()
        {
            TestMatches("someSuperTypedArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericArray()
        {
            TestMatches("someUnboundedGenericArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsBoundedGenericArrayOfProvidedType()
        {
            TestMatches("someBoundedGenericArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiBoundedGenericArrayOfProvidedType()
        {
            TestMatches("someMultiBoundedGenericArrayQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericArrayOfOtherType()
        {
            TestMatches("someNonMatchingBoundedGenericArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsListOfProvidedType()
        {
            TestMatches("someListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsSubListOfProvidedType()
        {
            TestMatches("someSubListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsSuperListOfProvidedType()
        {
            TestMatches("someSuperListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsBoundedGenericListOfProvidedType()
        {
            TestMatches("someBoundedGenericListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericList()
        {
            TestMatches("someUnboundedGenericListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiBoundedGenericListOfProvidedType()
        {
            TestMatches("someMultiBoundedGenericListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericListOfOtherType()
        {
            TestMatches("someNonMatchingBoundedGenericListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedWildcardList()
        {
            TestMatches("someUnboundedWildcardListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsLowerBoundedWildcardList()
        {
            TestMatches("someLowerBoundedWildcardListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsUpperBoundedWildcardListOfProvidedType()
        {
            TestMatches("someUpperBoundedWildcardListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsWildcardListOfOtherType()
        {
            TestMatches("someNonMatchingUpperBoundedWildcardQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericUpperBoundedWildcardList()
        {
            TestMatches("someUnboundedGenericUpperBoundedWildcardListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsGenericUpperBoundedWildcardListOfProvidedType()
        {
            TestMatches("someGenericUpperBoundedWildcardListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiGenericUpperBoundedWildcardListOfProvidedType()
        {
            TestMatches("someMultiGenericUpperBoundedWildcardListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsListImplementationOfProvidedType()
        {
            TestMatches("someListImplementationQuery", Matches);
        }

        /*
         This dummy function (QueryResponseList someListImplementationQuery) and dummy class (QueryResponseList) are
         contained in this test class instead of the AbstractResponseTypeTest class, because the functionality to derive
         whether a response type has a direct super type which we service (an Iterable in this case), checks if the
         enclosing classes contain unresolved generic types. It does this to check whether the type has raw types or not.
         Since the AbstractResponseTypeTest has a generic type R for test implementations, a check by that functionality for
         AbstractResponseTypeTest.QueryResponseList results in the state that it thinks it's unresolved
         (whilst in fact it is). This is however such a slim scenario, that I decided to put the dummy class and test
         function in the actual test class itself instead of in the abstract test class.
         */

        

        public static QueryResponseList someListImplementationQuery()
        {
            return new QueryResponseList();
        }

        @SuppressWarnings("WeakerAccess")

        static class QueryResponseList

        extends ArrayList<QueryResponse> {
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedListImplementationOfProvidedType()
        {
            TestMatches("someUnboundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsBoundedListImplementationOfProvidedType()
        {
            TestMatches("someBoundedListImplementationQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMultiUnboundedListImplementationOfProvidedType()
        {
            TestMatches("someMultiUnboundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiBoundedListImplementationOfProvidedType()
        {
            TestMatches("someMultiBoundedListImplementationQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsSetOfProvidedType()
        {
            TestMatches("someSetQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsStreamOfProvidedType()
        {
            TestMatches("someStreamQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMapOfProvidedType()
        {
            TestMatches("someMapQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsFutureOfProvidedType()
        {
            TestMatches("someFutureQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsListOfFutureOfProvidedType()
        {
            TestMatches("someFutureListQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsOptionalOfProvidedType()
        {
            TestMatches("someOptionalQueryResponse", DoesNotMatch);
        }

        

        [Fact]
        void testConvertThrowsExceptionForSingleInstanceResponse()
        {
            Assert.Throws<Exception>(() -> testSubject.convert(new QueryResponse()));
        }

        [Fact]
        void testConvertReturnsListOnResponseOfArrayType()
        {
            QueryResponse[] testResponse = new QueryResponse[] {new QueryResponse()};

            List<QueryResponse> result = testSubject.convert(testResponse);

            assertEquals(testResponse.length, result.size());
            assertEquals(testResponse[0], result.get(0));
        }

        [Fact]
        void testConvertReturnsListOnResponseOfSubTypedArrayType()
        {
            SubTypedQueryResponse[] testResponse = new SubTypedQueryResponse[] {new SubTypedQueryResponse()};

            List<QueryResponse> result = testSubject.convert(testResponse);

            assertEquals(testResponse.length, result.size());
            assertEquals(testResponse[0], result.get(0));
        }

        

        [Fact]
        void testConvertThrowsExceptionForResponseOfDifferentArrayType()
        {
            QueryResponseInterface[] testResponse = new QueryResponseInterface[]
            {
                new QueryResponseInterface()
                {
                }
            };

            assertThrows(Exception.class, () -> testSubject.convert(testResponse));
        }

        [Fact]
        void testConvertReturnsListOnResponseOfListType()
        {
            List<QueryResponse> testResponse = new ArrayList<>();
            testResponse.add(new QueryResponse());

            List<QueryResponse> result = testSubject.convert(testResponse);

            assertEquals(testResponse.size(), result.size());
            assertEquals(testResponse.get(0), result.get(0));
        }

        [Fact]
        void testConvertReturnsListOnResponseOfSubTypedListType()
        {
            List<SubTypedQueryResponse> testResponse = new ArrayList<>();
            testResponse.add(new SubTypedQueryResponse());

            List<QueryResponse> result = testSubject.convert(testResponse);

            assertEquals(testResponse.size(), result.size());
            assertEquals(testResponse.get(0), result.get(0));
        }

        [Fact]
        void testConvertReturnsListOnResponseOfSetType()
        {
            Set<QueryResponse> testResponse = new HashSet<>();
            testResponse.add(new QueryResponse());

            List<QueryResponse> result = testSubject.convert(testResponse);

            assertEquals(testResponse.size(), result.size());
            assertEquals(testResponse.iterator().next(), result.get(0));
        }

        

        [Fact]
        void testConvertThrowsExceptionForResponseOfDifferentListType()
        {
            List<QueryResponseInterface> testResponse = new ArrayList<>();
            testResponse.add(new QueryResponseInterface()
            {
            });

            assertThrows(Exception.class, () -> testSubject.convert(testResponse));
        }

        [Fact]
        void testConvertReturnsEmptyListForResponseOfDifferentListTypeIfTheListIsEmpty()
        {
            List<QueryResponseInterface> testResponse = new ArrayList<>();

            List<QueryResponse> result = testSubject.convert(testResponse);

            assertTrue(result.isEmpty());
        }
    }
}