using JsonApi.Envelope;

namespace JsonApi.Wrapper
{
    public interface IUnwrapper 
    {
        /// <summary>
        /// Unwrap a resource into a new object of type T.  The type must have a parameter-less constructor.
        /// </summary>
        /// <typeparam name="T">The type of the plain old CLR object wrapped in the resource.</typeparam>
        /// <param name="resource">A resource, typically the <see cref="IResourceEnvelope{T}.Data"/> of an envelope.</param>
        /// <returns>A newly created object of type T.</returns>
        T Unwrap<T>(Resource<T> resource) where T : class;

        /// <summary>
        /// Merge a resource into an existing object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the plain old CLR object wrapped in the resource.</typeparam>
        /// <param name="entity">The CLR object into which the wrapped data is merged.</param>
        /// <param name="resource">A resource, typically the <see cref="IResourceEnvelope{T}.Data"/> of an envelope.</param>
        void Merge<T>(T entity, Resource<T> resource) where T : class;
    }
}