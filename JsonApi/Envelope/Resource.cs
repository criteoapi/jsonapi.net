using System;
using Newtonsoft.Json;

namespace JsonApi.Envelope
{
    /// <summary>
    /// A data transfer object (DTO) for one POCO object of type T.
    /// </summary>
    /// <typeparam name="T">The type of the contained object.</typeparam>
    public class Resource<T> where T : class
    {
        /// <summary>
        /// Create a resource that holds a POCO object for serialization
        /// </summary>
        /// <param name="attributes">The object to be held</param>
        [JsonConstructor]
        public Resource(T attributes)
        {
            // Resource is not responsible for setting Type, Id, .... That is done in Wrapper
            // Attributes may be null if this is a reference resource (just type and id).
            Attributes = attributes;
        }

        /// <summary>
        /// The serializable type of the object
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The id of the object (must be distinct for distinct objects or null)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The object being held
        /// </summary>
        public T Attributes { get; }

        /// <summary>
        /// Links related to the object
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Links Links { get; set; }

        /// <summary>
        /// Meta information related to the object (e.g. ETAG)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Meta Meta { get; set; }
    }
}