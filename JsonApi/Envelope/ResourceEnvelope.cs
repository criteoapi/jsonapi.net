namespace JsonApi.Envelope
{
    public class ResourceEnvelope<T> : Envelope, IResourceEnvelope<T> where T : class
    {
        public Resource<T> Data { get; set; }
    }
}