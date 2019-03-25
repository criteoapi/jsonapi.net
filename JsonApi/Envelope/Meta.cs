using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonApi.Envelope
{
    public class Meta : Dictionary<string, object>
    {
        [JsonConstructor]
        public Meta()
        {
        }

        public Meta(IDictionary<string, object> resourceMeta) : base(resourceMeta)
        {
        }
    }
}