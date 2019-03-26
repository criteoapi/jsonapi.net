using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable ClassNeverInstantiated.Global

namespace JsonApi.Envelope
{
    /// <summary>
    /// ApiErrorsEnvelope encapsulates a response with errors.
    /// </summary>
    /// <typeparam name="T">The type of Resource being held.</typeparam>
    public class ApiErrorsEnvelope : Envelope, IApiErrorsEnvelope
    {
        /// <summary>
        /// Errors detected by the server.
        /// </summary>
        public IEnumerable<ApiError> Errors { get; set; }
    }
}