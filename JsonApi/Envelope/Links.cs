using System.Collections.Generic;

namespace JsonApi.Envelope
{
    /// <summary>
    /// Links encapsulate the links related to a envelope or resource.
    /// </summary>
    /// <inheritdoc cref="Dictionary{TKey,TValue}"/>
    public class Links : Dictionary<string, string>
    {
        private const string CanonicalKey = "canonical";
        private const string SelfKey = "self";
        private const string DefaultValue = null;
        
        public string Self
        {
            get => TryGetValue(SelfKey, out var value) ? value : DefaultValue;
            set => this[SelfKey] = value;
        }

        public string Canonical
        {
            get => TryGetValue(CanonicalKey, out var value) ? value : DefaultValue;
            set => this[CanonicalKey] = value;
        }
    }
}