using System.Linq.Expressions;

namespace PTrampert.QueryObjects.Internals
{
    internal class ParameterReplacementVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _sourceParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterReplacementVisitor(ParameterExpression sourceParameter, ParameterExpression newParameter)
        {
            _sourceParameter = sourceParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _sourceParameter ? _newParameter : node;
        }
    }
}