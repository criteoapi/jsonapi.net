﻿using System.Collections.Generic;
using JsonApi.Envelope;

namespace JsonApi.Wrapper
{
    public interface IWrapper
    {
        string ServerPath { get; }
        IResourceEnvelope<T> Wrap<T>(T entity) where T : class;
        IResourceCollectionEnvelope<T> WrapAll<T>(IEnumerable<T> entities, int size = 0) where T : class;
        IErrorsEnvelope Errors(IEnumerable<ApiError> entities);
    }
}