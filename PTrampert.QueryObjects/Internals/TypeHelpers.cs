using System;
using System.Collections;
using System.Collections.Concurrent;

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
    }
}