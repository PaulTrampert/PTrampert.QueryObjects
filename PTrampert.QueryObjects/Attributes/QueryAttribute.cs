using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PTrampert.QueryObjects.Attributes
{
    /// <summary>
    /// Attribute that marks a property as a filter value for a query.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class QueryAttribute : Attribute
    {
        /// <summary>
        /// The name of the property on the data model that this query property should be applied to.
        /// </summary>
        public string TargetProperty { get; private set; }

        /// <summary>
        /// Initialize the QueryAttribute, optionally overriding the target property name.
        /// </summary>
        /// <param name="targetProperty">
        /// The name of the property on the data model that this query property should be applied to. If not provided,
        /// the name of the query property will be used.
        /// </param>
        protected QueryAttribute(string targetProperty = null)
        {
            TargetProperty = targetProperty;
        }

        /// <summary>
        /// Builds the query expression for this query property.
        /// </summary>
        /// <param name="queryObject">The instance of the query object.</param>
        /// <param name="queryProperty">The PropertyInfo of the property this attribute is applied to.</param>
        /// <param name="targetParameter">The parameter expression describing the underlying database object.</param>
        /// <param name="targetProperty">The property on the targetParameter to compare against.</param>
        /// <returns>The query expression.</returns>
        public abstract Expression BuildExpression(object queryObject, PropertyInfo queryProperty,
            ParameterExpression targetParameter, PropertyInfo targetProperty);
    }
}