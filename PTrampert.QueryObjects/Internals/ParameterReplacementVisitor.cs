using System.Linq.Expressions;

namespace PTrampert.QueryObjects;

internal class ParameterReplacementVisitor(ParameterExpression sourceParameter, ParameterExpression newParameter) : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == sourceParameter ? newParameter : node;
    }
}