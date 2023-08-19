using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Org.GenericEntity.Model
{
    /// <summary>
    /// Reflection extensions
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Scans assemblies for custom attributes.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to search for.</typeparam>
        /// <param name="assembly">Assembly which will be searched.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Types which are decorated by custom attribute matching given predicate
        /// </returns>
        public static IEnumerable<AttributeInstance<T>> ScanTypesForCustomAttributes<T>(this Assembly assembly, Func<T, bool> predicate = null)
            where T : Attribute
        {
            foreach (AttributeInstance<T> attributeInstance in assembly.GetTypes().ScanTypesForCustomAttributes<T>(predicate))
            {
                yield return attributeInstance;
            }
        }

        /// <summary>
        /// Scans types for custom attributes.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to search for.</typeparam>
        /// <param name="types">Types which will be searched.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Types which are decorated by custom attribute matching given predicate</returns>
        public static IEnumerable<AttributeInstance<T>> ScanTypesForCustomAttributes<T>(this IEnumerable<Type> types, Func<T, bool> predicate = null)
            where T : Attribute
        {
            if (predicate != null)
            {
                return types
                    .Where(t => t.GetCustomAttribute<T>() != null && predicate(t.GetCustomAttribute<T>()))
                    .Select(t => new AttributeInstance<T>(t, t.GetCustomAttribute<T>()));
            }
            else
            {
                return types
                    .Where(t => t.GetCustomAttribute<T>() != null)
                    .Select(t => new AttributeInstance<T>(t, t.GetCustomAttribute<T>()));
            }
        }
    }
}