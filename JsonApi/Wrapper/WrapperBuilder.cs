using System;
using System.Collections.Generic;

namespace JsonApi.Wrapper
{
    public class WrapperBuilder :
        IExpectDefaultConfigOrTypeConfigWrapperBuilder,
        IExpectTypeConfigWrapperBuilder
    {
        /// <summary>
        /// Generic configuration to be applied to types as a default behavior.
        /// </summary>
        private TypeConfig DefaultTypeConfig { get; set; } = new TypeConfig();

        /// <summary>
        /// Resource type specific configuration.
        /// </summary>
        private Dictionary<Type, TypeConfig> TypeConfigs { get; set; } = new Dictionary<Type, TypeConfig>();

        /// <summary>
        /// The default root of the API used to compute resource paths.
        /// This may be overriden on a per resource basis.
        /// </summary>
        private string ServerPath { get; set; }


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
            IPolicyAsserter asserter;
            policyAsserter(asserter);
            return this;
        }

        public IExpectTypeConfigWrapperBuilder WithTypeConfig<T>(Action<IPolicyAsserter> policyAsserter)
        {
            var tt = typeof(T);
            if (!TypeConfigs.ContainsKey(tt)) TypeConfigs[tt] = new TypeConfig();
            TypeConfigs[tt].AddAll(policyAsserter);
            return this;
        }

        public Wrapper Build()
        {
            TypeConfigs[typeof(object)] = DefaultTypeConfig;
            return new Wrapper(ServerPath, TypeConfigs);
        }
    }

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