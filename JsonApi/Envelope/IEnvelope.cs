using System.Collections.Generic;

namespace JsonApi.Envelope
{
    /// <summary>
    /// An interface which defines the common behavior of all
    /// three envelope types.
    /// </summary>
    public interface IEnvelope
    {
        /// <summary>
        /// Dictionary of links to named related URI addresses. Some
        /// link keys are standard ("self", "canonical", and the set
        /// of keys used for paging.)  Others can be application
        /// defined.  All relative addresses are relative to the
        /// service root URI.
        /// </summary>
        Links Links { get; }

        /// <summary>
        /// Dictionary of non-business meta information regarding the
        /// envelope. Keys are strings whose semantics are defined by
        /// the application. 
        /// </summary>
        Meta Meta { get; }
    }

    /// <summary>
    /// An interface that defines the behavior of an envelope holding
    /// a single resource. 
    /// </summary>
    /// <typeparam name="T">The type wrapped by the resource.</typeparam>
    public interface IResourceEnvelope<T> : IEnvelope where T : class
    {
        /// <summary>
        /// The primary data being held by the resource envelope.
        /// </summary>
        /// <typeparam name="T">The type of the resource being held.</typeparam>
        Resource<T> Data { get; }
    }

    /// <summary>
    /// An interface that defines the behavior of an envelope holding
    /// a collection of resources.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the resource.</typeparam>
    public interface IResourceCollectionEnvelope<T> : IEnvelope where T : class
    {
        /// <summary>
        /// The primary data being held by the collection envelope
        /// </summary>
        /// <typeparam name="T">The type of all resources being held.</typeparam>
        IEnumerable<Resource<T>> Data { get; }
    }

    /// <summary>
    /// An interface that defines the behavior of an envelope holding
    /// one or more errors. 
    /// </summary>
    public interface IErrorsEnvelope : IEnvelope
    {
        IEnumerable<ApiError> Errors { get; }
    }
}