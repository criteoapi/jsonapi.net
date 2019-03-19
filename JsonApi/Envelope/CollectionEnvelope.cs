using System.Collections.Generic;

namespace JsonApi.Envelope
{
    public class CollectionEnvelope<T> : Envelope, IResourceCollectionEnvelope<T> where T : class
    {
        /// <inheritdoc/>
        public IEnumerable<Resource<T>> Data { get; set; }
    }
}