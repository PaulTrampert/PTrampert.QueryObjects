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
        /// Embed the query value into the expression tree as a literal constant instead of referencing it
        /// through the query object. Defaults to false.
        /// </summary>
        /// <remarks>
        /// By default the query value is referenced as a member access on the query object, which is the same
        /// shape the C# compiler emits for a captured variable. ORMs such as Entity Framework Core lift that
        /// shape into a SQL parameter, so the generated SQL is identical no matter what value is supplied, which
        /// keeps both the ORM's compiled query cache and the database's plan cache effective.
        ///
        /// A literal constant is inlined into the generated SQL, producing different SQL for every value. That is
        /// occasionally what you want -- for example when a predicate runs over badly skewed data and you would
        /// rather the database build a plan for the specific value than reuse a plan built for a previous one.
        /// </remarks>
        public bool InlineValue { get; set; }

        /// <summary>
        /// Builds the expression that supplies the query value to a comparison, honoring <see cref="InlineValue"/>.
        /// </summary>
        /// <param name="queryObject">The instance of the query object.</param>
        /// <param name="queryProperty">The PropertyInfo of the property holding the query value.</param>
        /// <param name="queryValue">
        /// The value of <paramref name="queryProperty"/> on <paramref name="queryObject"/>. Only used when
        /// <see cref="InlineValue"/> is true.
        /// </param>
        /// <param name="valueType">
        /// The type the resulting expression must have, or null to use the declared type of the query property.
        /// </param>
        /// <returns>The expression supplying the query value.</returns>
        protected Expression BuildValueExpression(object queryObject, PropertyInfo queryProperty, object queryValue,
            Type valueType = null)
        {
            if (InlineValue)
            {
                return valueType == null
                    ? Expression.Constant(queryValue)
                    : Expression.Constant(queryValue, valueType);
            }

            Expression value = Expression.Property(Expression.Constant(queryObject), queryProperty);
            if (valueType != null && !valueType.IsAssignableFrom(value.Type))
            {
                value = Expression.Convert(value, valueType);
            }

            return value;
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