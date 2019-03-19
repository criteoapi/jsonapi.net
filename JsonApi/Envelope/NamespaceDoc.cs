namespace JsonApi.Envelope
{
    /// <summary>
    /// <para>
    /// An envelope is a standard scheme for JSON documents that 
    /// enabled identification, paging, and linking of resources.
    /// </para>
    /// <para>
    /// There are three types of envelopes for the three types of
    /// response documents supported by JSON API. They differ in the  
    /// type of data that they can hold. 
    /// </para>
    /// <para>
    /// * resource   - holds a single resource
    /// * collection - holds a list of resources
    /// * error      - holds a list of api errors
    /// </para>
    /// <para>
    /// The resource and collection envelopes are also used by
    /// clients when sending resources in POST, PATCH and PUT
    /// requests. A resource is an object with a type and id. 
    /// In addition a resource may optionally contain any of
    /// </para>
    /// the following dictionaries (keys are always strings): 
    /// <para>
    /// * attributes - holds business data (objects)
    /// * links      - holds links (URI values) to related resources
    /// * meta       - holds meta-information (non-business data)
    /// </para>
    /// <para>
    /// Note: most REST authorities consider collections to be a
    /// resource. JSON API uses the term 'collection' for any
    /// endpoint that returns a collection of identically typed
    /// and individually addressable resources.
    /// </para>
    /// </summary>
    public class NamespaceDoc
    {
        
    }
}