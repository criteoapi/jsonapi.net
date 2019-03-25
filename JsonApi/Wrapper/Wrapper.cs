using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly Type DefaultType = typeof(object);

        /// <summary>
        /// The default wrapper. 
        /// </summary>
        public static Wrapper JsonApi { get; internal set; }

        /// <summary>
        /// Generic configuration to be applied to types as a default behavior.
        /// </summary>
        internal IPolicy DefaultTypeConfig => TypeConfigs[DefaultType];

        /// <summary>
        /// Resource type specific configuration.  Used as a cache for newly created policies.
        /// </summary>
        internal IDictionary<Type, IPolicy> TypeConfigs { get; }

        /// <summary>
        /// The default root of the API used to compute resource paths.
        /// This may be overriden on a per resource basis.
        /// </summary>
        public string ServerPath { get; }

        /// <summary>
        /// Wrapper ctor is internal to force the use of <see cref="WrapperBuilder"/> class
        /// and it's fluent interface for configuration.
        /// </summary>
        internal Wrapper(string serverPath, Dictionary<Type, IPolicy> typeConfigs)
        {
            ServerPath = serverPath; // builder guarantees that serverPath is non-empty
            TypeConfigs = typeConfigs; // builder guarantees a default type config
        }

        /// <summary>
        /// Get policy for a type. Policy will be created using default asserts if type
        /// has not been configured explicitly. The result will be cached. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IPolicy GetOrCreatePolicy(Type type)
        {
            if (!TypeConfigs.ContainsKey(type))
            {
                var asserter = ((Policy) TypeConfigs[DefaultType]).CopyAsserter();
                TypeConfigs[type] = new PolicyBuilder(type, asserter).Build();
            }

            return TypeConfigs[type];
        }

        private static Resource<T> ApplyPolicy<T>(Resource<T> resource, IPolicy policy) where T : class
        {
            var entity = resource.Attributes;
            resource.Type = policy.ResourceType(entity);
            resource.Id = policy.ResourceIdentity(entity);
            resource.Links = (Links)policy.ResourceLinks(entity);
            resource.Meta = (Meta)policy.ResourceMeta(entity);
            return resource;
        }

        public IResourceEnvelope<T> Wrap<T>(T entity) where T : class
        {
            var policy = GetOrCreatePolicy(typeof(T));
            var envelope = new ResourceEnvelope<T>(ApplyPolicy(new Resource<T>(entity), policy));

            return envelope;
        }

        public IResourceCollectionEnvelope<T> WrapAll<T>(IEnumerable<T> entities, int size = -1) where T : class
        {
            var policy = GetOrCreatePolicy(typeof(T)); // TODO: handle missing type: generate new policy

            var envelope = new CollectionEnvelope<T>(entities, entity => ApplyPolicy(new Resource<T>(entity), policy));
            if (size >= 0) envelope.Meta["count"] = size; // Do not enumerate all entities

            return envelope;
        }

        public IErrorsEnvelope Errors(IEnumerable<ApiError> entities)
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