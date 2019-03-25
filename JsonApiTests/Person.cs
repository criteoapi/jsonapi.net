// Types used in testing JSON serialization. 

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonApiTests
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    // [JsonObjectContract(typeof(DefaultContractResolver))]
    public class Person {
        public long Id { get; set; }            // choose as identifier - automatic

        [JsonProperty(PropertyName = "firstname")]
        public string RenameName { get; set; }        // map to "firstname" - manual
        [JsonIgnore]
        public string HiddenName{ get; set; }      // suppress - manual
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Field;                    // suppress if default - manual
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Address DefaultAddress { get; set; }            // show as embedded structure
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Error)]
        public Person ReferenceLoopManager { get; set; }     // show remote id (and type?) only

        // Private fields will not be marshaled
        private string PrivateProperty { get; set; } = "hidden";   // hide - not public
        private string _privateField = "hidden";            // hide - not public - IGNORE WARNING
    }

    public struct Address {
        public string PrivateGetter { private get; set; }  // hide - no public getter
        public string City { get; set; }        // map to "city"
        public State State { get; set; }        // map to "state", display enum values
        public int DefaultValue;
        public int? DefaultNullable;                    // suppress if default value - nullable
        public string DefaultClass { get; set; }    // suppress if default value - class 
    }

    public enum State { XX, AL, AR, AK, MI }
}