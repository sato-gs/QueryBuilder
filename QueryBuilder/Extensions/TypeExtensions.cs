// <copyright file="TypeExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.Extensions
{
    using System;
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// Contains extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Evaluates whether or not the given type is the <see cref="IEnumerable"/> type.
        /// </summary>
        /// <param name="type">The type to be evaluated.</param>
        /// <param name="includeStringAsEnumerable">The flag indicating whether or not the <see cref="string"/> type should be included as the <see cref="IEnumerable"/> type.</param>
        /// <returns>true/false if the given type is/is not the <see cref="IEnumerable"/> type.</returns>
        public static bool IsTypeEnumerable(this Type type, bool includeStringAsEnumerable = false)
        {
            if (includeStringAsEnumerable)
            {
                return type != null && type.GetInterfaces().Contains(typeof(IEnumerable));
            }
            else
            {
                return type != null && type != typeof(string) && type.GetInterfaces().Contains(typeof(IEnumerable));
            }
        }

        /// <summary>
        /// Evaluates whether or not the given type is the <see cref="Nullable"/> type.
        /// </summary>
        /// <param name="type">The type to be evaluated.</param>
        /// <returns>true/false if the given type is/is not the <see cref="Nullable"/> type.</returns>
        public static bool IsTypeNullable(this Type type)
        {
            return type == null || !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Converts the given type to the <see cref="Nullable"/> type.
        /// </summary>
        /// <param name="type">The type to be converted.</param>
        /// <returns>The converted type.</returns>
        public static Type ConvertToNullableType(this Type type)
        {
            return type.IsTypeNullable() ? type : typeof(Nullable<>).MakeGenericType(type);
        }

        /// <summary>
        /// Returns the non-Enumerable base type of the given type.
        /// </summary>
        /// <param name="type">The type to be used.</param>
        /// <returns>The non-Enumerable base type.</returns>
        public static Type GetNonEnumerableBaseType(this Type type)
        {
            if (type?.IsGenericType == true)
            {
                return type.GetGenericArguments().FirstOrDefault();
            }
            else
            {
                return type.IsTypeEnumerable() ? type.GetElementType() : type;
            }
        }

        /// <summary>
        /// Returns the non-Nullable base type of the given type.
        /// </summary>
        /// <param name="type">The type to be used.</param>
        /// <returns>The non-Nullable base type.</returns>
        public static Type GetNonNullableBaseType(this Type type)
        {
            return type == null ? type : Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Returns the non-Enumerable non-Nullable base type of the given type.
        /// </summary>
        /// <param name="type">The type to be used.</param>
        /// <returns>The non-Enumerable non-Nullable base type.</returns>
        public static Type GetNonEnumerableNonNullableBaseType(this Type type)
        {
            return type.GetNonEnumerableBaseType().GetNonNullableBaseType();
        }
    }
}