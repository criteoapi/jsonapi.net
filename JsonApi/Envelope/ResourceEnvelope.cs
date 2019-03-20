using System;

namespace JsonApi.Envelope
{
    public class ResourceEnvelope<T> : Envelope, IResourceEnvelope<T> where T : class
    {
        public ResourceEnvelope(Resource<T> data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public ResourceEnvelope(T poco) : this(new Resource<T>(poco))
        {
        }

        public Resource<T> Data { get; }
    }
}