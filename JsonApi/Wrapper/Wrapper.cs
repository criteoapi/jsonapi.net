using System;
using System.Collections.Generic;
using JsonApi.Envelope;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// Wrapper is responsible for wrapping and unwrapping plain CLR objects
    /// for the purpose of marshalling the wrapped objects to JSON following
    /// the conventions of <a href="http://jsonapi.org">JSON:API</a>.
    /// </summary>
    /// <seealso cref="WrapperBuilder"/>
    public sealed class Wrapper : IWrapper, IUnwrapper
    {
        /// <summary>
        /// The default wrapper. 
        /// </summary>
        public static Wrapper JsonApi { get; internal set; }

        /// <summary>
        /// Generic configuration to be applied to types as a default behavior.
        /// </summary>
        internal TypeConfig DefaultTypeConfig => TypeConfigs[typeof(object)];

        /// <summary>
        /// Resource type specific configuration.
        /// </summary>
        internal IReadOnlyDictionary<Type, TypeConfig> TypeConfigs { get; }

        /// <summary>
        /// The default root of the API used to compute resource paths.
        /// This may be overriden on a per resource basis.
        /// </summary>
        public string ServerPath { get; }

        /// <summary>
        /// Wrapper ctor is internal to force the use of <see cref="WrapperBuilder"/> class
        /// and it's fluent interface for configuration.
        /// </summary>
        internal Wrapper(string serverPath, Dictionary<Type, TypeConfig> typeConfigs)
        {
            // builder will ensure that serverPath is non-empty
            ServerPath = serverPath;
            // builder type will ensure that the null key of the
            // dictionary is populated with a default type config
            TypeConfigs = typeConfigs;
        }

        public IResourceEnvelope<T> Wrap<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public IResourceCollectionEnvelope<T> Wrap<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public IErrorsEnvelope Wrap(IEnumerable<ApiError> entities)
        {
            throw new NotImplementedException();
        }

        public T Unwrap<T>(Resource<T> resource) where T : class
        {
            throw new NotImplementedException();
        }

        public void Merge<T>(T entity, Resource<T> resource) where T : class
        {
            throw new NotImplementedException();
        }
    }
}