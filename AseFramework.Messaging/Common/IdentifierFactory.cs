using System;

namespace Ase.Messaging.Common
{
    /// <summary>
    /// Abstract Factory class that provides access to an IdentifierFactory implementation. The IdentifierFactory
    /// is responsible for generated unique identifiers for domain objects, such as Aggregates (AggregateRoot) and Events.
    /// <p/>
    /// This class uses the {@link ServiceLoader} mechanism to find implementations. If none are found, it defaults to an
    /// implementation that provides randomly chosen {@code java.util.UUID}s.
    /// <p/>
    /// To provide your own implementation, create a file called {@code org.axonframework.common.IdentifierFactory} in
    /// the {@code META-INF/services} package. The file must contain the fully qualified class name of the
    /// implementation to use. This implementation must have a public no-arg constructor and extend IdentifierFactory.
    /// <p/>
    /// This class is thread safe to use.
    /// </summary>
    /// @see ServiceLoader
    public abstract class IdentifierFactory
    {
        private static readonly Lazy<IdentifierFactory> Instance =
            new Lazy<IdentifierFactory>(() =>
            {
                // logger.debug("Looking for IdentifierFactory implementation using the context class loader");
                IdentifierFactory? factory = null;//locateFactories(Thread.currentThread().getContextClassLoader(), "Context");
                if (factory == null) {
                    // logger.debug("Looking for IdentifierFactory implementation using the IdentifierFactory class loader.");
                    factory = null;//locateFactories(IdentifierFactory.class.getClassLoader(), "IdentifierFactory");
                }
                if (factory == null) {
                    factory = new DefaultIdentifierFactory();
                    // logger.debug("Using default UUID-based IdentifierFactory");
                } else {
                    // logger.info("Found custom IdentifierFactory implementation: {}", factory.getClass().getName());
                }

                return factory;
            });

        /// <summary>
        /// Returns an instance of the IdentifierFactory discovered on the classpath. This class uses the {@link
        /// ServiceLoader} mechanism to find implementations. If none are found, it defaults to an implementation that
        /// provides randomly chosen {@code java.util.UUID}s.
        /// </summary>
        /// <returns>the IdentifierFactory implementation found on the classpath.</returns>
        public static IdentifierFactory GetInstance()
        {
            return Instance.Value;
        }

        /// <summary>
        /// Generates a unique identifier for use by Entities (generally the Aggregate Root) and Events. The implementation
        /// may choose whatever strategy it sees fit, as long as the chance of a duplicate identifier is acceptable to the
        /// application.
        /// </summary>
        /// <returns>a String representation of a unique identifier</returns>
        public abstract string GenerateIdentifier();
    }
}