using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonApi.Envelope
{
    /// <summary>
    /// A wrapper for one resource.
    /// Every resource must have a Type and unless it is being created
    /// by the client in a POST, PUT, or PATCH request, it must have an Id. 
    /// </summary>
    public class Resource<T> where T : class
    {
        public string Type{ get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id{ get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Attributes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Links Links { get; set; }
    }

    public class Attributes : Dictionary<string, object>
    {
    }
}