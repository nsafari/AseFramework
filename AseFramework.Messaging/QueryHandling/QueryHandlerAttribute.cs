using System;

namespace Ase.Messaging.QueryHandling
{
    
    /// <summary>
    /// Marker annotation to mark any method on an object as being a QueryHandler. Use the {@link
    /// org.axonframework.queryhandling.annotation.AnnotationQueryHandlerAdapter AnnotationQueryHandlerAdapter} to subscribe
    /// the annotated class to the query bus.
    /// <p>
    /// The annotated method's first parameter is the query handled by that method. Optionally, the query handler may
    /// specify a second parameter of type {@link org.axonframework.messaging.MetaData}. The active MetaData will be
    /// passed if that parameter is supplied.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryHandlerAttribute: Attribute
    {
        /// <summary>
        /// The name of the Query this handler listens to. Defaults to the fully qualified class name of the payload type
        /// (i.e. first parameter).
        ///
        /// @return The query name
        /// </summary>
        private string QueryName { get; set; } = "";
        
    }
}