using System;
using System.Linq.Expressions;

namespace Sketch7.Core.ExpressionUtils
{
	public class ParameterToMemberExpressionRebinder : ExpressionVisitor
	{
		private readonly ParameterExpression _paramExpr;
		private readonly MemberExpression _memberExpr;

		public ParameterToMemberExpressionRebinder(ParameterExpression paramExpr, MemberExpression memberExpr)
		{
			_paramExpr = paramExpr;
			_memberExpr = memberExpr;
		}

		public override Expression Visit(Expression p) => base.Visit(p == _paramExpr ? _memberExpr : p);

		public static Expression<Func<T, bool>> CombinePropertySelectorWithPredicate<T, T2>(
			Expression<Func<T, T2>> propertySelector,
			Expression<Func<T2, bool>> propertyPredicate)
		{
			if (!(propertySelector.Body is MemberExpression memberExpression))
			{
				throw new ArgumentException("propertySelector");
			}

			var expr = Expression.Lambda<Func<T, bool>>(propertyPredicate.Body, propertySelector.Parameters);
			var rebinder = new ParameterToMemberExpressionRebinder(propertyPredicate.Parameters[0], memberExpression);
			expr = (Expression<Func<T, bool>>) rebinder.Visit(expr);

			return expr;
		}
	}
}