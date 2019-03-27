using Newtonsoft.Json;

namespace JsonApi.Envelope
{
    /// <summary>
    /// Envelope contains common features of all envelope types.
    /// It acts as a base class.
    /// </summary>
    public class Envelope : IEnvelope
    {
        private Meta _meta;
        private Links _links;

        /// <summary>
        /// Meta information related to the primary data.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Meta Meta => _meta ?? (_meta = new Meta());

        /// <summary>
        /// Links related to the primary data.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Links Links => _links ?? (_links = new Links());

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.None);
    }
}