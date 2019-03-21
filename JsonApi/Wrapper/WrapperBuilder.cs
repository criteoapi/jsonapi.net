using System;
using System.Collections.Generic;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// A fluent API to create a JSON:API Wrapper.
    /// </summary>
    /// <example>
    ///  JsonApiWrapper = JsonApi.WrapperBuilder
    ///      .WithServer(rootUri.ToString())
    ///      .WithDefaultConfig(p => {
    ///          p.PluralTypes();
    ///          p.WithId("Id");
    ///          p.HideDefaults();
    ///      })
    ///      .WithTypeConfig&lt;Person>(p => {
    ///          p.AtUri("people");
    ///          p.WithId("PersonId");
    ///          p.ReplaceName("Name","firstname");
    ///          p.ReplaceName("Last", "lastname");
    ///          p.HideName("SSN");
    ///          p.HideName("DateOfBirth");
    ///      })
    ///      .WithTypeConfig&lt;Address>(p => {
    ///          p.ReplaceName("Line1","street");
    ///          p.ShowDefault("Country");
    ///      })
    ///      .Build();
    /// </example>
    public class WrapperBuilder :
        IExpectDefaultConfigOrTypeConfigWrapperBuilder
    {
        private readonly Type _defaultType = typeof(object);

        /// <summary>
        /// Default policy assertions if not specified by <see cref="WithDefaultConfig"/>
        /// </summary>
        private Action<IPolicyAsserter> _baseConfig = p =>
        {
            p.PluralTypes();
            p.CamelCasedNames();
            p.HideDefaults();
            p.WithId("Id"); // TODO: EndsWith("Id") ? 
        };

        /// <summary>
        /// Builders for resource type specific configuration. The same builder may
        /// be reused by multiple calls to <see cref="WithTypeConfig{T}"/>.
        /// </summary>
        private Dictionary<Type, PolicyBuilder> PolicyBuilders { get; } = new Dictionary<Type, PolicyBuilder>();

        /// <summary>
        /// The default root of the API used to compute resource paths.
        /// This may be overriden on a per resource basis.
        /// </summary>
        private string ServerPath { get; }


        /// <summary>
        /// WrapperBuilder ctor is private. Use static entry method <see cref="WithServer"/>.
        /// </summary>
        /// <param name="serverPath"></param>
        private WrapperBuilder(string serverPath)
        {
            ServerPath = serverPath;

            // Add a default policy to apply as a base for other types
            PolicyBuilders[_defaultType] = new PolicyBuilder(_defaultType);
            var asserter = PolicyBuilders[_defaultType].Asserter;
            _baseConfig(asserter);
        }

        public static IExpectDefaultConfigOrTypeConfigWrapperBuilder WithServer(string serverPath)
        {
            if (serverPath == null) throw new ArgumentNullException(nameof(serverPath));
            return new WrapperBuilder(serverPath);
        }

        public IExpectTypeConfigWrapperBuilder WithDefaultConfig(Action<IPolicyAsserter> policyAsserter)
        {
            if (!PolicyBuilders.ContainsKey(_defaultType)) // add to existing policy if possible
            {
                PolicyBuilders[_defaultType] = new PolicyBuilder(_defaultType);
            }
            IPolicyAsserter asserter = PolicyBuilders[_defaultType].Asserter;

            _baseConfig = policyAsserter; // save base config to apply to specific types
            policyAsserter(asserter);
            return this;
        }

        public IExpectTypeConfigWrapperBuilder WithTypeConfig<T>(Action<IPolicyAsserter> policyAsserter)
        {
            var tt = typeof(T);
            if (!PolicyBuilders.ContainsKey(tt))
            {
                PolicyBuilders[tt] = new PolicyBuilder(typeof(T));
            }
            IPolicyAsserter asserter = PolicyBuilders[tt].Asserter;

            _baseConfig(asserter);
            policyAsserter(asserter);
            return this;
        }

        public Wrapper Build()
        {
            //
            // Build TypeConfigs from the PolicyBuilders
            Dictionary<Type, IPolicy> typeConfigs = new Dictionary<Type, IPolicy>();
            foreach (KeyValuePair<Type, PolicyBuilder> policyBuilder in PolicyBuilders)
            {
                typeConfigs[policyBuilder.Key] = policyBuilder.Value.Build();
            }
            return new Wrapper(ServerPath, typeConfigs);
        }
    }

    // Interfaces used to make fluid API

    public interface IExpectDefaultConfigOrTypeConfigWrapperBuilder :
        IExpectTypeConfigWrapperBuilder
    {
        IExpectTypeConfigWrapperBuilder WithDefaultConfig(Action<IPolicyAsserter> policyAsserter);
    }

    public interface IExpectTypeConfigWrapperBuilder
    {
        IExpectTypeConfigWrapperBuilder WithTypeConfig<T>(Action<IPolicyAsserter> policyAsserter);
        Wrapper Build();
    }
}