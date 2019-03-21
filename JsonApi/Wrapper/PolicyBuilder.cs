using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// PolicyBuilder generates a single Policy for a type from
    /// a sequence of IPolicyAsserter calls.
    /// </summary>
    public class PolicyBuilder
    {
        private readonly PolicyData _policyData;

        /// <summary>
        /// The object against which policy assertions are made.
        /// </summary>
        internal PolicyBuilder.PolicyAsserter Asserter = new PolicyBuilder.PolicyAsserter();
        

        /// <summary>
        /// Create a default policy builder that acts as the base policy for all types
        /// </summary>
        public PolicyBuilder() : this(typeof(object))
        {
        }

        /// <summary>
        /// Create a policy builder for a policy that applies to type 
        /// </summary>
        /// <param name="type">type for which policy is being asserted</param>
        /// <param name="parent">parent policy from which to inherit behavior</param>
        public PolicyBuilder(Type type, Policy parent = null)
        {
            _policyData = new PolicyData(type);
            if (parent != null)
            {
                // TODO: copy parent policy results to this builder
            }

            _policyData = new PolicyData(type);
        }

        public Policy Build()
        {
            var policy = new Policy(_policyData);
            return policy;
        }

        internal class PolicyAsserter : IPolicyAsserter
        {

            #region IPolicyAsserter implementation

            public void HyphenCasedTypes() => _asserts.Add(new HyphenCasedTypesAssertion());

            public void PluralTypes() => throw new NotImplementedException();

            public void WithType(string name) => throw new NotImplementedException();

            public void WithId(string name) => throw new NotImplementedException();

            public void WithSelfLinks() => throw new NotImplementedException();

            public void WithCanonicalLinks() => throw new NotImplementedException();

            public void CamelCasedNames() => throw new NotImplementedException();

            public void ReplaceName(string name, string key) => throw new NotImplementedException();

            public void HideDefaults() => throw new NotImplementedException();

            public void HideName(string name) => throw new NotImplementedException();

            public void HideDefault(string name) => throw new NotImplementedException();

            public void ShowDefault(string name) => throw new NotImplementedException();

            #endregion

            #region Assertion Objects

            private List<Assertion> _asserts = new List<Assertion>();

            internal abstract class Assertion
            {
                public AssertionType AssertType { get; }
                public readonly string[] Args;

                protected Assertion(AssertionType type, params string[] args)
                {
                    AssertType = type;
                    Args = args;
                }

                internal abstract void Execute();
            }

            internal enum AssertionType
            {
                Unknown, Root, Link, Meta, Data, Hide, Rename
            }

            internal class HyphenCasedTypesAssertion : Assertion
            {
                public HyphenCasedTypesAssertion() : base(AssertionType.Root) { }

                internal override void Execute()
                {

                }
            }

            /// <summary>
            /// Convert the list of assertions for this type to
            /// type configuration data.
            /// </summary>
            private void ProcessAssertions()
            {
                _asserts.ForEach(a => a.Execute());
            }

            #endregion
        }
    }
}