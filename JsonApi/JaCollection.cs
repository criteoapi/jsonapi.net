using System.Collections.Generic;
using System.Linq;

namespace JsonApi
{

    public class JaCollection
    {
        public string Type { get; set; }

        public string Id { get; set; }

        public IEnumerable<object> Data { get; set; }

        public IDictionary<string, string> Links { get; set; }

        public IDictionary<string, string> Meta { get; set; }
    }
}