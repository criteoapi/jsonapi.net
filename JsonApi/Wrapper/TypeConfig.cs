using System.Collections.Generic;

namespace JsonApi.Wrapper
{
    public class TypeConfig
    {
        public List<IPolicy> Policies { get; } = new List<IPolicy>();

        public void AddAll(IEnumerable<IPolicy> policies)
        {
            Policies.AddRange(policies);
        }
    }
}