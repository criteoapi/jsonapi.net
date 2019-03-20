using System.Collections.Generic;

namespace JsonApi.Envelope
{
    /// <summary>
    /// General purpose attributes class that can be used for deserialization of objects
    /// whose type is not configured.  Useful for testing as well.
    /// </summary>
    public class Attributes : Dictionary<string, object>
    {
    }
}