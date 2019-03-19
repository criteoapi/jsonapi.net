using System.Collections.Generic;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// Defines behavior for wrapping and unwrapping resources of a specific type.
    /// Every resource type has a separate policy object.
    /// </summary>
    public interface IPolicy
    {
        /// <summary>
        /// Return the resource type from the POCO type based on the Type and Identity policies.
        /// </summary>
        /// <returns>A string that will act as the type value.</returns>
        string ResourceType();

        /// <summary>
        /// Return the Id from the POCO based on the Type and Identity policies.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A string that will act as the type value.</returns>
        string ResourceIdentity(object obj);

        /// <summary>
        /// Return a table of links for the POCO based on the Link policies.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, string> ResourceLinks(object obj);

        /// <summary>
        /// Return a table of meta information for the POCO based on the Meta policies.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, string> ResourceMeta(object obj);

        /// <summary>
        /// Return a table of attributes for the POCO based on the Suppression and Name Handling policies.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, string> Attributes(object obj);
    }
}