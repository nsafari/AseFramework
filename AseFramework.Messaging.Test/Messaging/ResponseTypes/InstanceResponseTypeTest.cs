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
        
        

    }
}