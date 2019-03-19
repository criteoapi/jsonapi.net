using System;
using System.Collections.Generic;

namespace JsonApi.Envelope
{
    /// <summary>
    /// ApiError holds an error encountered on the server. 
    /// </summary>
    public class ApiError : Meta
    {
        /// <summary>
        /// Internal error code that can be of use to the server developer.
        /// Developers must ensure that implementation details are not leaked. 
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// HTTP status that may not be reflected as response status
        /// in the event of multiple server errors.
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Details to help the user or client developer correct the error.
        /// 
        /// Details are not expected to be localized and may include varying
        /// text that is derived from the request (e.g. MIME type in Accept).
        /// Details can assume the location in <see cref="Source"/> as context.
        /// </summary>
        /// <example>"Not authorized for active budgets. Create a new budget."</example>
        public string Detail { get; }

        /// <summary>
        /// Location in the request document that most closely pinpoints the
        /// source of the error.  Keys may include "header", "pointer", "path",
        /// and "query" to identify the location type.  Values depend on the type.
        ///
        /// * Header uses the name of the header to identify an invalid request header. 
        /// * Pointer uses jpointer semantics to identify an object in the request body.
        /// * Path uses the segment name to identify an invalid request URI path segment.
        /// * Query uses the name of the query parameter to identify a malformed parameter.
        /// </summary>
        public Dictionary<string, string> Source { get; private set; }

        /// <summary>
        /// Create a new ApiError object.
        /// </summary>
        /// <param name="code">Internal error code</param>
        /// <param name="status">HTTP Status code</param>
        /// <param name="detail">Details intended for client developers</param>
        public ApiError(string code, string status, string detail)
        {
            Code = code;
            Status = status;
            Detail = detail;
        }

        /// <summary>
        /// Add a source key value pair to the dictionary.
        /// </summary>
        /// <param name="key">Location type (e.g. header, pointer, path, or query).</param>
        /// <param name="value">Type specific location identifier.</param>
        private void AddSource(string key, string value)
        {
            if (Source == null)
            {
                Source = new Dictionary<string, string>();
            }

            if (key == null) throw new ArgumentNullException(nameof(key));
            Source[key] = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}