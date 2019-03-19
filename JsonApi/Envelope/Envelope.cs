using Newtonsoft.Json;

namespace JsonApi.Envelope
{
    /// <summary>
    /// Envelope contains common features of all envelope types.
    /// It acts as a base class.
    /// </summary>
    public class Envelope : IEnvelope
    {
        /// <summary>
        /// Meta information related to the primary data.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Meta Meta { get; set; } = new Meta();

        /// <summary>
        /// Links related to the primary data.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Links Links { get; set; } = new Links();

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.None);
    }
}