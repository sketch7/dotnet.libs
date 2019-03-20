using System.Linq.Expressions;

namespace Sketch7.Core.ExpressionUtils
{
	public class ParameterReplaceVisitor : ExpressionVisitor
	{
		private readonly ParameterExpression _from, _to;

		public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
		{
			_from = from;
			_to = to;
		}

		protected override Expression VisitParameter(ParameterExpression node) => node == _from ? _to : base.VisitParameter(node);
	}
}