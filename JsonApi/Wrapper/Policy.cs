using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// Data Transfer Object shared by Policy and PolicyBuilder
    /// </summary>
    public class PolicyData
    {
        /// <summary>
        /// The actual type of T
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Server root to use for canonical URI of external resources
        /// </summary>
        public string ServerPath { get; set; }

        /// <summary>
        /// These members will be used to generate an id value.
        /// </summary>
        internal MemberInfo[] IdMembers;

        /// <summary>
        /// These members will be exported as attributes (excludes id)
        /// </summary>
        internal MemberInfo[] AttributeMembers;

        /// <summary>
        /// These members will be exported as attributes unless value matches object
        /// </summary>
        internal Dictionary<MemberInfo, object> _nonDefaultAttributeMembers;

        /// <summary>
        /// The mapping of member to attribute name.
        /// </summary>
        internal Dictionary<MemberInfo, string> _replaceName;

        public PolicyData(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }

    /// <summary>
    /// Policy defines the wrapping behavior for a specific type. 
    /// </summary>
    public class Policy : IPolicy
    {
        private readonly PolicyData _policyData;
        
        /// <summary>
        /// Create a policy with specified behavior for a type.
        /// This constructor is only used by <see cref="PolicyBuilder"/>.
        /// </summary>
        internal Policy(PolicyData policyData)
        {
            _policyData = policyData;
        }

        #region IPolicy implementation

        /// <param name="obj"></param>
        /// <inheritdoc cref="IPolicy"/>
        public string ResourceType(object obj)
        {
            return _policyData.Type.Name;
        }

        /// <summary>
        /// Return the Id from the POCO based on the Type and Identity policies.
        /// </summary>
        /// <param name="obj">POCO source</param>
        /// <returns>A string that will act as the type value.</returns>
        public string ResourceIdentity(object obj)
        {
            if (_policyData.IdMembers.Length == 0) throw new InvalidOperationException($"Type {_policyData.Type} has no identifier members");

            var first = _policyData.IdMembers[0].GetValue(obj).ToString();
            if (_policyData.IdMembers.Length == 1) return first;
            return _policyData.IdMembers.Aggregate(first, (prev, m) => prev + "." + m.GetValue(obj));
        }

        /// <summary>
        /// Get the links proper to the POCO or null if none
        /// </summary>
        /// <param name="obj">POCO source</param>
        /// <param name="baseUri">base URI for absolute URI paths or null</param>
        /// <returns>Dictionary of links mapped to URI values</returns>
        public IDictionary<string, string> ResourceLinks(object obj, string baseUri = null)
        {
            baseUri = _policyData.ServerPath ?? baseUri ?? "";
            if (!string.IsNullOrEmpty(baseUri)) baseUri += "/";
            var links = new Dictionary<string, string>();

            // TODO: make this conditional
            links.Add("canonical", $"{baseUri}{ResourceType(obj)}/{ResourceIdentity(obj)}");

            return links.Count > 0 ? links : null;
        }

        /// <summary>
        /// Get the metadata of the POCO or null if none
        /// </summary>
        /// <param name="obj">POCO source</param>
        /// <returns>Dictionary of metadata names mapped to object values</returns>
        public IDictionary<string, object> ResourceMeta(object obj)
        {
            var meta = new Dictionary<string, object>();

            // TODO: future use for ETAG, creation date, etc. 
            meta.Add("meta", "data");

            return meta.Count > 0 ? meta : null;
        }

        /// <summary>
        /// Get the attributes from the POCO using name mapping and suppression policies.
        /// Not used for serialization since that is done by Swashbuckle today.
        /// </summary>
        /// <param name="obj">POCO source</param>
        /// <returns>Dictionary of attributes</returns>
        public IDictionary<string, string> Attributes(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}