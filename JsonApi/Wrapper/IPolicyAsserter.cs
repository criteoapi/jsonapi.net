namespace JsonApi.Wrapper
{
    public interface IPolicyAsserter
    {
        #region Type and identity policies

        /// <summary>
        /// use lowercase with hyphens instead of `nameof(T)`, for example use
        /// `hyphen-cased` if the type name is `HyphenCased`.
        /// </summary>
        IPolicyAsserter HyphenCasedTypes();

        /// <summary>
        /// use the plural form of `nameof(T)` as the type and the URI segment
        /// in the canonical URI path for the type.  Only the last word of a
        /// multi-word   type is pluralized (e.g. `seller-budgets` for
        /// `SellerBudgets`.)
        /// </summary>
        IPolicyAsserter PluralTypes();

        /// <summary>
        /// explicitly define the type and URI segment.
        /// </summary>
        IPolicyAsserter WithType(string name);

        /// <summary>
        /// explicitly define the attribute that holds the id.
        /// </summary>
        IPolicyAsserter WithId(string name);

        #endregion

        #region Linking Policies

        /// <summary>
        /// include "self" links in the <see cref="Resource{T}"/> links dictionary
        /// </summary>
        IPolicyAsserter WithSelfLinks();

        /// <summary>
        /// include "canonical" links in the <see cref="Resource{T}"/> links dictionary
        /// </summary>
        IPolicyAsserter WithCanonicalLinks();

        #endregion

        #region Name Handling Policies

        /// <summary>
        /// use lower camel case for property and field names, for example
        /// `camelCased` for the property `CamelCased`.
        /// </summary>
        IPolicyAsserter CamelCasedNames();

        /// <summary>
        /// explicitly use `key` for the property `name`.
        /// </summary>
        IPolicyAsserter ReplaceName(string name, string key);

        #endregion

        #region Suppression Policies

        /// <summary>
        /// if a property or field has its default value, do not show it.
        /// </summary>
        IPolicyAsserter HideDefaults();

        /// <summary>
        /// never show this property or field.
        /// </summary>
        IPolicyAsserter HideName(string name);

        /// <summary>
        /// if the property or field `name` has its default value, do not show
        /// it.
        /// </summary>
        IPolicyAsserter HideDefault(string name);

        /// <summary>
        /// if the property or field `name` has its default value show it.
        /// </summary>
        IPolicyAsserter ShowDefault(string name);

        #endregion
    }
}