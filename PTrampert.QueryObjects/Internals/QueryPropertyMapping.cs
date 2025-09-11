using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PTrampert.QueryObjects.Attributes;

namespace PTrampert.QueryObjects.Internals
{
    internal class QueryPropertyMapping
    {
        public PropertyInfo QueryProperty { get; set; }
        public QueryAttribute QueryAttribute { get; set; }
        public PropertyInfo TargetProperty { get; set; }
    }
}