using System;

namespace Ase.Messaging.Common
{
    /// <summary>
    /// Default IdentifierFactory implementation that uses generates random {@code java.util.UUID} based identifiers.
    /// </summary>
    public class DefaultIdentifierFactory : IdentifierFactory
    {
        public override string GenerateIdentifier()
        {
            return Guid.NewGuid().ToString();
        }
    }
}