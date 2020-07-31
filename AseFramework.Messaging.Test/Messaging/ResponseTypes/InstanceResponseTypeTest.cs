using System;
using Ase.Messaging.Messaging.ResponseTypes;
using Xunit;

namespace Ase.Messaging.Test.Messaging.ResponseTypes
{
    /// <summary>
    /// Test all possible permutations of Query Handler return types through the {@link InstanceResponseType}. To that end,
    /// leveraging the  {@link AbstractResponseTypeTest} to cover all usual suspects between the different
    /// {@link ResponseType} implementations.
    /// </summary>
    public class InstanceResponseTypeTest : AbstractResponseTypeTest<QueryResponse>
    {
        public InstanceResponseTypeTest() : base(new InstanceResponseType<QueryResponse>(typeof(QueryResponse)))
        {
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsTheSame()
        {
            TestMatches("SomeQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsSubTypeOfProvidedType()
        {
            TestMatches("SomeSubTypedQuery", Matches);
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
        void TestMatchesReturnsTrueIfResponseTypeIsBoundedGenericOfProvidedType()
        {
            TestMatches("SomeBoundedGenericQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsMultiBoundedGenericOfProvidedType()
        {
            TestMatches("SomeMultiBoundedGenericQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericOfOtherType()
        {
            TestMatches("SomeNonMatchingBoundedGenericQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsArrayOfProvidedType()
        {
            TestMatches("SomeArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsArrayWithSubTypeOfProvidedType()
        {
            TestMatches("SomeSubTypedArrayQuery", DoesNotMatch);
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
        void TestMatchesReturnsFalseIfResponseTypeIsBoundedGenericArrayOfProvidedType()
        {
            TestMatches("SomeBoundedGenericArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMultiBoundedGenericArrayOfProvidedType()
        {
            TestMatches("SomeMultiBoundedGenericArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericArrayOfOtherType()
        {
            TestMatches("SomeNonMatchingBoundedGenericArrayQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsListOfProvidedType()
        {
            TestMatches("SomeListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsSubListOfProvidedType()
        {
            TestMatches("SomeSubListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsSuperListOfProvidedType()
        {
            TestMatches("SomeSuperListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsBoundedGenericListOfProvidedType()
        {
            TestMatches("SomeBoundedGenericListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericList()
        {
            TestMatches("SomeUnboundedGenericListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMultiBoundedGenericListOfProvidedType()
        {
            TestMatches("SomeMultiBoundedGenericListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsGenericListOfOtherType()
        {
            TestMatches("SomeNonMatchingBoundedGenericListQuery", DoesNotMatch);
        }

        // [Fact]
        // void TestMatchesReturnsFalseIfResponseTypeIsUnboundedWildcardList()
        // {
        //     TestMatches("SomeUnboundedWildcardListQuery", DoesNotMatch);
        // }

        // [Fact]
        // void TestMatchesReturnsFalseIfResponseTypeIsLowerBoundedWildcardList()
        // {
        //     TestMatches("SomeLowerBoundedWildcardListQuery", DoesNotMatch);
        // }

        // [Fact]
        // void TestMatchesReturnsFalseIfResponseTypeIsUpperBoundedWildcardListOfProvidedType()
        // {
        //     TestMatches("SomeUpperBoundedWildcardListQuery", DoesNotMatch);
        // }

        // [Fact]
        // void TestMatchesReturnsFalseIfResponseTypeIsWildcardListOfOtherType()
        // {
        //     TestMatches("SomeNonMatchingUpperBoundedWildcardQuery", DoesNotMatch);
        // }

        // [Fact]
        // void TestMatchesReturnsFalseIfResponseTypeIsUnboundedGenericUpperBoundedWildcardList()
        // {
        //     TestMatches("SomeUnboundedGenericUpperBoundedWildcardListQuery", DoesNotMatch);
        // }

        // [Fact]
        // void TestMatchesReturnsFalseIfResponseTypeIsGenericUpperBoundedWildcardListOfProvidedType()
        // {
        //     TestMatches("SomeGenericUpperBoundedWildcardListQuery", DoesNotMatch);
        // }

        // [Fact]
        // void TestMatchesReturnsFalseIfResponseTypeIsMultiGenericUpperBoundedWildcardListOfProvidedType()
        // {
        //     TestMatches("SomeMultiGenericUpperBoundedWildcardListQuery", DoesNotMatch);
        // }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsUnboundedListImplementationOfProvidedType()
        {
            TestMatches("SomeUnboundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsBoundedListImplementationOfProvidedType()
        {
            TestMatches("SomeBoundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMultiUnboundedListImplementationOfProvidedType()
        {
            TestMatches("SomeMultiUnboundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMultiBoundedListImplementationOfProvidedType()
        {
            TestMatches("SomeMultiBoundedListImplementationQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsSetOfProvidedType()
        {
            TestMatches("SomeSetQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsStreamOfProvidedType()
        {
            TestMatches("SomeStreamQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsMapOfProvidedType()
        {
            TestMatches("SomeMapQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsFutureOfProvidedType()
        {
            TestMatches("SomeFutureQuery", Matches);
        }

        [Fact]
        void TestMatchesReturnsFalseIfResponseTypeIsListOfFutureOfProvidedType()
        {
            TestMatches("SomeFutureListQuery", DoesNotMatch);
        }

        [Fact]
        void TestMatchesReturnsTrueIfResponseTypeIsOptionalOfProvidedType()
        {
            TestMatches("SomeOptionalQueryResponse", Matches);
        }
        
        [Fact]
        void TestConvertReturnsSingleResponseAsIs() {
            QueryResponse testResponse = new QueryResponse();

            QueryResponse result = TestSubject.Convert(testResponse);

            Assert.Equal(testResponse, result);
        }
        
        
        [Fact]
        void TestConvertReturnsSingleResponseAsIsForSubTypedResponse() {
            SubTypedQueryResponse testResponse = new SubTypedQueryResponse();

            QueryResponse result = TestSubject.Convert(testResponse);

            Assert.Equal(testResponse, result);
        }
        
        [Fact]
        void TestConvertThrowsClassCastExceptionForDifferentSingleInstanceResponse()
        {
            // IQueryResponseInterface q = (IQueryResponseInterface) new { };
            // Assert.Throws(typeof(Exception), () => {
                // QueryResponse convert = TestSubject.Convert(q);
            // });
        }

        [Fact]
        void TestConvertThrowsClassCastExceptionForMultipleInstanceResponse() {
            Assert.Throws(typeof(Exception), () => {
                QueryResponse convert = TestSubject.Convert(new QueryResponse[]{});
            });
        }
    }
}