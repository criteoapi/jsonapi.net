using System;
using System.Text.RegularExpressions;
using Pluralize.NET.Core;

namespace JsonApi.Wrapper
{
    public static class StringHelper
    {
        static Pluralizer PluralizationService { get; } = new Pluralizer();

        public static string ToPlural(this string name)
        {
            return PluralizationService.Pluralize(name);
        }

        public static string ToSingular(this string name)
        {
            return PluralizationService.Singularize(name);
        }

        /// <summary>
        /// Convert to camel case.
        /// Author: <a href="https://stackoverflow.com/a/50066302/4089171">John Henckel</a> on SO
        /// Examples: Location_ID => LocationId, and testLEFTSide => TestLeftSide
        /// </summary>
        public static string CamelCase(this string name)
        {
            name = name.Replace("_", "");
            if (name.Length == 0) return "Null";
            name = Regex.Replace(name, "([A-Z])([A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            return char.ToUpper(name[0]) + name.Substring(1);
        }

        /// <summary>
        /// Convert to underscore case.
        /// Examples: LocationId => Location_Id, and testLEFTSide => Test_Left_Side
        /// </summary>
        public static string ToHyphenCase(this string name) // TODO: complete implementation
        {
            if (name.Length == 0) return "Null";
            name = Regex.Replace(name, "([a-z]|[A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + "_" + m.Groups[2].Value);
            var len = name[name.Length - 1] == '_' ? name.Length - 1 : name.Length; 
            return name.Substring(0, len).ToLower();
        }
    }
}