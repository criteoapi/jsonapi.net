using System.Collections.Generic;
using JsonApi.Envelope;

namespace JsonApi.Wrapper
{
    public interface IWrapper
    {
        string ServerPath { get; }
        ResourceEnvelope<T> Wrap<T>(T entity) where T : class;
        CollectionEnvelope<T> WrapAll<T>(IEnumerable<T> entities, int size = 0) where T : class;
        ApiErrorsEnvelope Errors(IEnumerable<ApiError> entities);
    }
}