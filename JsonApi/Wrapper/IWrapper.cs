using System.Collections.Generic;
using JsonApi.Envelope;

namespace JsonApi.Wrapper
{
    public interface IWrapper
    {
        IResourceEnvelope<T> Wrap<T>(T entity) where T : class;
        IResourceCollectionEnvelope<T> Wrap<T>(IEnumerable<T> entities) where T : class;
        IErrorsEnvelope Wrap(IEnumerable<ApiError> entities);
    }
}