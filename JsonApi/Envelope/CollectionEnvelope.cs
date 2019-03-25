using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonApi.Envelope
{
    public class CollectionEnvelope<T> : Envelope, IResourceCollectionEnvelope<T> where T : class
    {
        private readonly IEnumerable<T> _entities;

        private readonly Func<T, Resource<T>> _wrapFunc = entity => new Resource<T>(entity);

        public CollectionEnvelope(IEnumerable<T> entities)
        {
            _entities = entities;
        }

        public CollectionEnvelope(IEnumerable<T> entities, Func<T, Resource<T>> wrapFunc)
        {
            _entities = entities;
            _wrapFunc = wrapFunc;
        }

        /// <inheritdoc/>
        public IEnumerable<Resource<T>> Data => _entities.Select(entity => _wrapFunc(entity));
    }
}