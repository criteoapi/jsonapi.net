using System;
using System.Collections.Generic;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// PolicyBuilder generates a single Policy for a type from
    /// a sequence of IPolicyAsserter calls.
    /// </summary>
    public class PolicyBuilder
    {
        /// <summary>
        /// Data used to enforce the policy when wrapping or unwrapping.
        /// </summary>
        private readonly PolicyData _policyData;

        public IPolicyAsserter Asserter => _policyData.Asserter;

        /// <summary>
        /// Default policy assertions if not specified by <see cref="WrapperBuilder.WithDefaultConfig"/>.
        /// </summary>
        private readonly Action<IPolicyAsserter> _defaultAsserts = p =>
        {
            p.PluralTypes();
            p.HyphenCasedTypes();
            p.CamelCasedNames();
            p.HideDefaults();
            p.WithId("Id"); // TODO: EndsWith("Id") ? 
        };

        /// <summary>
        /// Create a policy builder that applies to the default type
        /// </summary>
        public PolicyBuilder()
        {
            _policyData = new PolicyData(typeof(object), new PolicyAsserter());
            _defaultAsserts?.Invoke(_policyData.Asserter);
        }

        /// <summary>
        /// Create a policy builder for a policy that applies to type 
        /// </summary>
        /// <param name="type">type for which policy is being asserted</param>
        /// <param name="initialAsserts">parent policy from which to inherit behavior</param>
        public PolicyBuilder(Type type, IPolicyAsserter initialAsserts)
        {
            _policyData = new PolicyData(type, initialAsserts.Copy());
        }

        /// <summary>
        /// Create a Policy from the assertions that were supplied.
        /// </summary>
        /// <returns>A new Policy</returns>
        public Policy Build()
        {
            ((PolicyAsserter)_policyData.Asserter).ProcessAssertions(_policyData);

            // If TypeName (e.g. "people") was not supplied by a policy generate from Type
            if (_policyData.TypeName == null)
            {
                var name = _policyData.Type.Name;
                name = _policyData.PluralTypes ? name.ToPlural() : name;
                name = _policyData.HyphenCasedTypes ? name.ToHyphenCase() : name;
                _policyData.TypeName = name;
            }

            // 
            var policy = new Policy(_policyData);
            return policy;
        }

        /// <summary>
        /// Store asserts for later application to a PolicyData object.
        /// </summary>
        internal class PolicyAsserter : IPolicyAsserter
        {
            private readonly List<Assertion> _asserts;

            /// <summary>
            /// Create a policy asserter.
            /// </summary>
            public PolicyAsserter()
            {
                _asserts = new List<Assertion>();
            }

            /// <summary>
            /// Create a copy from the parent.
            /// </summary>
            /// <param name="parent"></param>
            private PolicyAsserter(PolicyAsserter parent)
            {
                _asserts = new List<Assertion>(parent._asserts);
            }

            public IPolicyAsserter Copy()
            {
                return new PolicyAsserter(this);
            }

            /// <summary>
            /// Apply stored assertions to policy data.
            /// </summary>
            /// <param name="policyData">PolicyData object to modify.</param>
            internal void ProcessAssertions(PolicyData policyData)
            {
                // Console.WriteLine($"Building Policy for {policyData.Type}.");
                _asserts.ForEach(a => a.Execute(policyData));
            }

            public void HyphenCasedTypes() => _asserts.Add(new Assertion(AssertionCategory.Root, nameof(HyphenCasedTypes)));

            public void PluralTypes() => _asserts.Add(new Assertion(AssertionCategory.Data, nameof(PluralTypes)));

            public void WithType(string name) => _asserts.Add(new Assertion(AssertionCategory.Data, nameof(WithType), name));

            public void WithId(params string[] names) => _asserts.Add(new Assertion(AssertionCategory.Data, nameof(WithId), names));

            public void WithSelfLinks() => _asserts.Add(new Assertion(AssertionCategory.Link, nameof(WithSelfLinks)));

            public void WithCanonicalLinks() => _asserts.Add(new Assertion(AssertionCategory.Link, nameof(WithCanonicalLinks)));

            public void CamelCasedNames() => _asserts.Add(new Assertion(AssertionCategory.Rename, nameof(CamelCasedNames)));

            public void ReplaceName(string name, string key) => _asserts.Add(new Assertion(AssertionCategory.Rename, nameof(ReplaceName), name, key));

            public void HideDefaults() => _asserts.Add(new Assertion(AssertionCategory.Hide, nameof(HideDefaults)));

            public void HideName(string name) => _asserts.Add(new Assertion(AssertionCategory.Hide, nameof(HideName), name));

            public void HideDefault(string name) => _asserts.Add(new Assertion(AssertionCategory.Hide, nameof(HideDefault), name));

            public void ShowDefault(string name) => _asserts.Add(new Assertion(AssertionCategory.Hide, nameof(ShowDefault), name));
        
            /// <summary>
            /// Command object that stores the method for modifying a policy data object from an assertion.
            /// </summary>
            internal class Assertion
            {
                public AssertionCategory Category { get; }
                public string Name { get; }
                public readonly string[] Args;

                internal Assertion(AssertionCategory category, string name, params string[] args)
                {
                    Category = category;
                    Name = name;
                    Args = args;
                }

                // Perfect use case for polymorphism that I was too lazy to implement (see switch).
                internal void Execute(PolicyData policyData)
                {
                    // TODO: implement assertions to modify PolicyData
                    switch (Name)
                    {
                        case nameof(PluralTypes):
                            policyData.PluralTypes = true;
                            break;

                        case nameof(HyphenCasedTypes):
                            policyData.HyphenCasedTypes = true;
                            break;

                        case nameof(WithType):
                            policyData.TypeName = Args[0];
                            break;

                        case nameof(WithId):
                            policyData.IdMembers = policyData.Type.FindFieldOrPropertyMembers(m => m.Name == Args[0]);
                            break;

                        case nameof(WithSelfLinks):
                            policyData.LinkNames["self"] = true;
                            break;

                        case nameof(WithCanonicalLinks):
                            policyData.LinkNames["canonical"] = true;
                            break;

                        case nameof(CamelCasedNames):
                            policyData.CamelCasedNames = true;
                            break;

                        case nameof(ReplaceName):
                            policyData.ReplaceName.Add(policyData.Type.FindMemberInfo(Args[0]), Args[1]);
                            break;

                        case nameof(HideDefaults):
                            break; // TODO

                        case nameof(HideName):
                            break; // TODO

                        case nameof(HideDefault):
                            break; // TODO

                        case nameof(ShowDefault):
                            break; // TODO

                        default:
                            throw new Exception($"Missing case {Name} in policy builder");
                    }
                }
            }

            internal enum AssertionCategory
            {
                Unknown, Root, Link, Meta, Data, Hide, Rename
            }
        }
    }
}