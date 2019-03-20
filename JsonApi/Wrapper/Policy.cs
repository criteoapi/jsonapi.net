using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// Policy defines the wrapping behavior for a specific type. 
    /// </summary>
    public class Policy : IPolicy
    {
        /// <summary>
        /// The actual type of T
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// These members will be used to generate an id value.
        /// </summary>
        private readonly MemberInfo[] _idMembers;

        /// <summary>
        /// These members will be exported as attributes, regardless of value
        /// </summary>
        private readonly MemberInfo[] _attributeMembers;

        /// <summary>
        /// These members will be exported as attributes, only if not default
        /// </summary>
        private readonly MemberInfo[] _nonDefaultAttributeMembers;

        /// <summary>
        /// The mapping of member to attribute name.
        /// </summary>
        private readonly Dictionary<MemberInfo, string> _replaceName;

        /// <summary>
        /// Create a policy with default behavior for a type.
        /// This constructor is used by <see cref="PolicyBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        internal Policy(Type type)
        {
            Type = type;
            _idMembers = type.FindFieldOrPropertyMembers(m => m.Name.EndsWith("Id"));
            _attributeMembers = type.FindFieldOrPropertyMembers(m => !m.Name.EndsWith("Id"));
            _nonDefaultAttributeMembers = new MemberInfo[]{};
            _replaceName = _attributeMembers.ToDictionary(m => m, m => m.Name.ToLowerInvariant());
        }

        /// <summary>
        /// Create a policy with specified behavior for a type.
        /// This constructor is used by <see cref="PolicyBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        internal Policy(Type type = null, MemberInfo[] idMembers = null, MemberInfo[] attributeMembers = null, MemberInfo[] nonDefaultAttributeMembers = null, Dictionary<MemberInfo, string> replaceName = null)
        {
            Type = type;
            _idMembers = idMembers;
            _attributeMembers = attributeMembers;
            _nonDefaultAttributeMembers = nonDefaultAttributeMembers;
            _replaceName = replaceName;
        }

        #region IPolicy implementation

        /// <inheritdoc cref="IPolicy"/>
        public string ResourceType()
        {
            return Type.Name;
        }

        public string ResourceIdentity(object obj)
        {
            if (_idMembers.Length == 0) throw new InvalidOperationException($"Type {Type} has no identifier members");

            var first = _idMembers[0].GetValue(obj).ToString();
            if (_idMembers.Length == 1) return first;
            return _idMembers.Aggregate(first, (prev, m) => prev + "." + m.GetValue(obj));
        }

        public IDictionary<string, string> ResourceLinks(object obj)
        {
            var links = new Dictionary<string, string>();
            links.Add("canonical", $"{ResourceType()}/{ResourceType()}");
            return links;
        }

        public IDictionary<string, string> ResourceMeta(object obj)
        {
            return new Dictionary<string, string>(); // TODO: check if meta is object valued
        }

        public IDictionary<string, string> Attributes(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}