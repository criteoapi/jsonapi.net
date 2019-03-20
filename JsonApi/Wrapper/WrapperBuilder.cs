using System;
using System.Collections.Generic;

namespace JsonApi.Wrapper
{
    public class WrapperBuilder :
        IExpectDefaultConfigOrTypeConfigWrapperBuilder,
        IExpectTypeConfigWrapperBuilder
    {
        private readonly Type _defaultType = typeof(object);

        /// <summary>
        /// Generic configuration to be applied to types as a default behavior.
        /// </summary>
        private PolicyBuilder DefaultPolicyBuilder =>
            PolicyBuilders.ContainsKey(_defaultType) 
                ? PolicyBuilders[_defaultType] 
                : new PolicyBuilder(_defaultType);

        /// <summary>
        /// Resource type specific configuration.
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
        }

        public static IExpectDefaultConfigOrTypeConfigWrapperBuilder WithServer(string serverPath)
        {
            if (serverPath == null) throw new ArgumentNullException(nameof(serverPath));
            return new WrapperBuilder(serverPath);
        }

        public IExpectTypeConfigWrapperBuilder WithDefaultConfig(Action<IPolicyAsserter> policyAsserter)
        {
            var tt = _defaultType;
            if (!PolicyBuilders.ContainsKey(tt))
            {
                PolicyBuilders[tt] = new PolicyBuilder(_defaultType);
            }
            IPolicyAsserter asserter = PolicyBuilders[tt];

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
            IPolicyAsserter asserter = PolicyBuilders[tt];

            policyAsserter(asserter);
            return this;
        }

        public Wrapper Build()
        {
            Dictionary<Type, IPolicy> typeConfigs = new Dictionary<Type, IPolicy>();
            // TODO: Build TypeConfigs from the PolicyBuilders
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