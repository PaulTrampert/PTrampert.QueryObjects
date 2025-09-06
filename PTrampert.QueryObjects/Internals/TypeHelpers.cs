using System;
using System.Collections;

namespace PTrampert.QueryObjects.Internals
{
    public static class TypeHelpers
    {
        public static Type GetCollectionElementType(this Type collectionType)
        {
            if (collectionType.IsArray)
            {
                return collectionType.GetElementType();
            }

            if (typeof(IEnumerable).IsAssignableFrom(collectionType))
            {
                if (collectionType.IsGenericType)
                {
                    return collectionType.GetGenericArguments()[0];
                }

                return typeof(object);
            }
            
            throw new ArgumentException($"The collection type '{collectionType.Name}' is not an array type.");
        }
    }
}