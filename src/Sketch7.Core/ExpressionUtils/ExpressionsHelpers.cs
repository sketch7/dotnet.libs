using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sketch7.Core.ExpressionUtils
{
	public class ExpressionsHelpers
	{
		public static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>(
			params Expression<Func<TSource, TDestination>>[] selectors)
		{
			var param = Expression.Parameter(typeof (TSource), "x");
			return Expression.Lambda<Func<TSource, TDestination>>(
				Expression.MemberInit(
					Expression.New(typeof (TDestination).GetConstructor(Type.EmptyTypes)),
					from selector in selectors
					let replace = new ParameterReplaceVisitor(
						selector.Parameters[0], param)
					from binding in ((MemberInitExpression) selector.Body).Bindings
						.OfType<MemberAssignment>()
					select Expression.Bind(binding.Member,
					                       replace.Visit(binding.Expression)))
										   // todo: check if is the same result
					                       //replace.VisitAndConvert(binding.Expression, "Combine")))
				, param);
		}

		//This is as the above however simplified.
		//  public static Expression<Func<TSource, TDestination>> CombineBig<TSource, TDestination>(
		//params Expression<Func<TSource, TDestination>>[] selectors)
		//  {
		//      var zeroth = ((MemberInitExpression)selectors[0].Body);
		//      var param = selectors[0].Parameters[0];
		//      List<MemberBinding> bindings = new List<MemberBinding>(zeroth.Bindings.OfType<MemberAssignment>());
		//      for (int i = 1; i < selectors.Length; i++)
		//      {
		//          var memberInit = (MemberInitExpression)selectors[i].Body;
		//          var replace = new ParameterReplaceVisitor(selectors[i].Parameters[0], param);
		//          foreach (var binding in memberInit.Bindings.OfType<MemberAssignment>())
		//          {
		//              bindings.Add(Expression.Bind(binding.Member,
		//                  replace.VisitAndConvert(binding.Expression, "Combine")));
		//          }
		//      }

		//      return Expression.Lambda<Func<TSource, TDestination>>(
		//          Expression.MemberInit(zeroth.NewExpression, bindings), param);
		//  }
	}
}