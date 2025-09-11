using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace PTrampert.QueryObjects.Internals
{
    internal static class TypeHelpers
    {
        private static readonly ConcurrentDictionary<Type, Type> CollectionElementTypes = new();
        
        public static Type GetCollectionElementType(this Type collectionType)
        {
            return CollectionElementTypes.GetOrAdd(collectionType, cType =>
            {
                if (cType.IsArray)
                {
                    return collectionType.GetElementType();
                }

                if (!typeof(IEnumerable).IsAssignableFrom(cType))
                    throw new ArgumentException($"The collection type '{cType.Name}' is not an array type.");
                
                return cType.IsGenericType ? cType.GetGenericArguments()[0] : typeof(object);
            });
        }
        
        private static readonly ConcurrentDictionary<Type, MethodInfo> ElementTypeToContainsMethod = new();

        public static MethodInfo GetContainsMethod(this Type elementType)
        {
            return ElementTypeToContainsMethod.GetOrAdd(elementType, eType =>
                typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                    .MakeGenericMethod(eType));
        }
        
        private static readonly ConcurrentDictionary<Type, MethodInfo> ElementTypeToIntersectMethod = new();
        
        public static MethodInfo GetIntersectMethod(this Type elementType)
        {
            return ElementTypeToIntersectMethod.GetOrAdd(elementType, eType =>
                typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Single(m => m.Name == nameof(Enumerable.Intersect) && m.GetParameters().Length == 2)
                    .MakeGenericMethod(eType));
        }
        
        private static readonly ConcurrentDictionary<Type, MethodInfo> ElementTypeToAnyMethod = new();
        
        public static MethodInfo GetAnyMethod(this Type elementType)
        {
            return ElementTypeToAnyMethod.GetOrAdd(elementType, eType =>
                typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Single(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 1)
                    .MakeGenericMethod(eType));
        }
    }
}