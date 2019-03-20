using System;
using System.Collections.Generic;

namespace JsonApi.Wrapper
{
    public class PolicyBuilder : IPolicyAsserter
    {
        private IPolicy _typeConfig;

        /// <summary>
        /// The type of the policy being built.
        /// </summary>
        public Type Type { get; }

        public List<IPolicy> Policies { get; }

        public PolicyBuilder(Type type)
        {
            Type = type;
        }

        public IPolicy Build()
        {
            return _typeConfig;
        }

        #region IPolicyAsserter implementation

        public IPolicyAsserter HyphenCasedTypes()
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter PluralTypes()
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter WithType(string name)
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter WithId(string name)
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter WithSelfLinks()
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter WithCanonicalLinks()
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter CamelCasedNames()
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter ReplaceName(string name, string key)
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter HideDefaults()
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter HideName(string name)
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter HideDefault(string name)
        {
            throw new NotImplementedException();
        }

        public IPolicyAsserter ShowDefault(string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}