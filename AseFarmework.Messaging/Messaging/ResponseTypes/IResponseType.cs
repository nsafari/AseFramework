namespace Ase.Messaging.Messaging.ResponseTypes
{
    /// <summary>
    /// Specifies the expected response type required when performing a query through the
    /// {@link org.axonframework.queryhandling.QueryBus}/{@link org.axonframework.queryhandling.QueryGateway}.
    /// By wrapping the response type as a generic {@code R}, we can easily service the expected response as a single
    /// instance, a list, a page etc., based on the selected implementation even while the query handler return type might be
    /// slightly different.
    /// <p>
    /// It is in charge of matching the response type of a query handler with the given generic {@code R}.
    /// If this match returns true, it signals the found query handler can handle the intended query.
    /// As a follow up, the response retrieved from a query handler should move through the
    /// {@link ResponseType#convert(Object)} function to guarantee the right response type is returned.
    /// <typeparam name="R">the generic type of this {@link ResponseType} to be matched and converted.</typeparam>
    /// <seealso cref="ResponseTypes"/>
    /// </summary>
    public interface IResponseType<R>
    {
        
    }
}