namespace JsonApi.Wrapper
{
    public interface IPolicyAsserter
    {
        IPolicyAsserter Copy();

        #region Type and identity policies

        /// <summary>
        /// use named server root for this type, rather than the default server root
        /// </summary>
        /// <param name="serverPath">absolute server root</param>
        void WithServerRoot(string serverPath);

        /// <summary>
        /// use lowercase with hyphens instead of `nameof(T)`, for example use
        /// `hyphen-cased` if the type name is `HyphenCased`.
        /// </summary>
        void HyphenCasedTypes();

        /// <summary>
        /// use the plural form of `nameof(T)` as the type and the URI segment
        /// in the canonical URI path for the type.  Only the last word of a
        /// multi-word   type is pluralized (e.g. `seller-budgets` for
        /// `SellerBudgets`.)
        /// </summary>
        void PluralTypes();

        /// <summary>
        /// explicitly define the type and URI segment.
        /// </summary>
        void WithType(string name);

        /// <summary>
        /// explicitly define the attribute(s) that holds the id.
        /// </summary>
        void WithId(params string[] name);

        #endregion

        #region Linking Policies

        /// <summary>
        /// include "self" links in the <see cref="Resource{T}"/> links dictionary
        /// </summary>
        void WithSelfLinks();

        /// <summary>
        /// include "canonical" links in the <see cref="Resource{T}"/> links dictionary
        /// </summary>
        void WithCanonicalLinks();

        #endregion

        #region Name Handling Policies

        /// <summary>
        /// use lower camel case for property and field names, for example
        /// `camelCased` for the property `CamelCased`.
        /// </summary>
        void CamelCasedNames();

        /// <summary>
        /// explicitly use `key` for the property `name`.
        /// </summary>
        void ReplaceName(string name, string key);

        #endregion

        #region Suppression Policies

        /// <summary>
        /// if a property or field has its default value, do not show it.
        /// </summary>
        void HideDefaults();

        /// <summary>
        /// never show this property or field.
        /// </summary>
        void HideName(string name);

        /// <summary>
        /// if the property or field `name` has its default value, do not show
        /// it.
        /// </summary>
        void HideDefault(string name);

        /// <summary>
        /// if the property or field `name` has its default value show it.
        /// </summary>
        void ShowDefault(string name);

        #endregion
    }
}