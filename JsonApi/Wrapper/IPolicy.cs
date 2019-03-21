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
        /// <param name="obj"></param>
        /// <returns>A string that will act as the type value.</returns>
        string ResourceType(object obj);

        /// <summary>
        /// Return the Id from the POCO based on the Type and Identity policies.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A string that will act as the type value.</returns>
        string ResourceIdentity(object obj);

        /// <summary>
        /// Return a table of links for the POCO based on the Link policies.
        /// </summary>
        /// <param name="obj">entity whose links are returned</param>
        /// <param name="baseURI">optional base URI to create absolute links</param>
        /// <returns></returns>
        IDictionary<string, string> ResourceLinks(object obj, string baseURI = null);

        /// <summary>
        /// Return a table of meta information for the POCO based on the Meta policies.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, object> ResourceMeta(object obj);

        /// <summary>
        /// Return a table of attributes for the POCO based on the Suppression and Name
        /// Handling policies. Attributes that are hidden/suppressed will not appear.
        /// TODO: Currently not used due to the need for scheme generation by reflection.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IDictionary<string, string> Attributes(object obj);
    }
}