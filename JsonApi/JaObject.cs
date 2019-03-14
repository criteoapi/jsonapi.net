using System.Collections.Generic;
using System.Linq;

namespace JsonApi
{
    public class JaObject {
        public string Type { get; set; }

        public string Id { get; set; }

        public IDictionary<string, object> Attributes { get; set; }

        public IDictionary<string, string> Links { get; set; }

        public IDictionary<string, string> Meta { get; set; }
    }
}