using System;
using System.Reflection;

namespace JsonApi.Wrapper
{
    public static class TypeHelper
    {
        private const string MemberInfoTypeError = "MemberInfo must be of type FieldInfo or PropertyInfo";

        // some logic borrowed from James Newton-King, http://www.newtonsoft.com
        public static void SetValue(this MemberInfo member, object property, object value)
        {
            if (member.MemberType == MemberTypes.Property)
                ((PropertyInfo)member).SetValue(property, value, null);
            else if (member.MemberType == MemberTypes.Field)
                ((FieldInfo)member).SetValue(property, value);
            else
                throw new ArgumentException(MemberInfoTypeError, nameof(member));
        }

        public static object GetValue(this MemberInfo member, object property)
        {
            if (member.MemberType == MemberTypes.Property)
                return ((PropertyInfo)member).GetValue(property, null);
            else if (member.MemberType == MemberTypes.Field)
                return ((FieldInfo)member).GetValue(property);
            else
                throw new ArgumentException(MemberInfoTypeError, nameof(member));
        }

        public static Type GetType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException(MemberInfoTypeError, nameof(member));
            }
        }

        /// <summary>
        /// Helper method that can be used to identify properties and fields of a type.
        /// All public instance fields and properties will be subjected to the filter.
        /// </summary>
        /// <param name="type">The Type whose members are being filtered.</param>
        /// <param name="filter">Predicate for filtering out specific members.</param>
        /// <returns>An array of matching members.</returns>
        public static MemberInfo[] FindFieldOrPropertyMembers(this Type type, Func<MemberInfo, bool> filter)
        {
            return type.FindMembers(
                MemberTypes.Field | MemberTypes.Property,
                BindingFlags.Public | BindingFlags.Instance,
                (info, _) => filter(info), null);
        }

        public static MemberInfo FindMemberInfo(this Type type, string name)
        {
            var members = type.GetMember(name, 
                MemberTypes.Field | MemberTypes.Property,
                BindingFlags.Public | BindingFlags.Instance);
            return members.Length > 0 ? members[0] : null;
        }
    }
}