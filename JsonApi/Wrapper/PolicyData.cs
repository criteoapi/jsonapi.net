﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace JsonApi.Wrapper
{
    /// <summary>
    /// Data Transfer Object shared by Policy and PolicyBuilder
    /// </summary>
    public class PolicyData
    {
        /// <summary>
        /// The actual type of T
        /// </summary>
        public Type Type { get; }

        #region Type and Identity Policy

        /// <summary>
        /// Server root to use for canonical URI of external resources, or null
        /// </summary>
        public string ServerRoot { get; set; }

        /// <summary>
        /// An explicit type name to use
        /// </summary>
        public string TypeName;

        /// <summary>
        /// Use hyphen_case for the type name
        /// </summary>
        public bool HyphenCasedTypes;

        /// <summary>
        /// Use plural for the type name (e.g. people, not person)
        /// </summary>
        public bool PluralTypes;

        /// <summary>
        /// These members will be used to generate an id value.
        /// </summary>
        internal MemberInfo[] IdMembers = { };

        #endregion

        #region Naming Policy

        /// <summary>
        /// These members will be exported as attributes (excludes id)
        /// </summary>
        internal MemberInfo[] AttributeMembers = {};

        /// <summary>
        /// Convert names not in <see cref="ReplaceName"/> with lower case camelcase version.
        /// </summary>
        public bool CamelCasedNames;

        /// <summary>
        /// The mapping of member to attribute name.
        /// </summary>
        internal Dictionary<MemberInfo, string> ReplaceName = new Dictionary<MemberInfo, string>();

        #endregion

        // Suppression Policy

        /// <summary>
        /// These members will be exported as attributes unless value matches object
        /// </summary>
        internal Dictionary<MemberInfo, bool> HideIfDefault = new Dictionary<MemberInfo, bool>();

        // Link and Meta Policy

        /// <summary>
        /// The links to generate in the links section of the resource. 
        /// </summary>
        public Dictionary<string, bool> LinkNames = new Dictionary<string, bool>();

        /// <summary>
        /// The asserter against which the policies were made.
        /// </summary>
        internal IPolicyAsserter Asserter;

        public PolicyData(Type type, IPolicyAsserter asserter)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Asserter = asserter ?? throw new ArgumentNullException(nameof(asserter));
        }
    }
}