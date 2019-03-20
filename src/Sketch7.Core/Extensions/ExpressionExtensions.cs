using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Sketch7.Core.ExpressionUtils;

[assembly: InternalsVisibleTo("Sketch7.Core.Entity")]

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	/// <summary>
	/// 	http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
	/// </summary>
	public static class ExpressionExtensions
	{
		public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
											   Func<Expression, Expression, Expression> merge)
		{
			// build parameter map (from parameters of second to parameters of first)
			var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

			// replace parameters in the second lambda expression with parameters from the first
			var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

			// apply composition of lambda expression bodies to parameters from the first expression 
			return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
		}

		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Compose(second, Expression.And);

		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Compose(second, Expression.Or);

		/// <summary>
		/// 	Extract list of <see cref="MemberInfo" /> from expression.
		/// </summary>
		/// <typeparam name="T"> The generic type. </typeparam>
		/// <param name="propertySelector"> </param>
		/// <returns> Returns the MemberInfo list. </returns>
		public static List<MemberInfo> ExtractMembers<T>(this Expression<T> propertySelector)
		{
			var members = new List<MemberInfo>();
			var lambda = propertySelector as LambdaExpression;
			MemberExpression memberExpression;
			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = lambda.Body as UnaryExpression;
				memberExpression = unaryExpression.Operand as MemberExpression;
				members.Add(memberExpression.Member);
			}
			else if (lambda.Body is NewExpression)
				members = ((NewExpression)propertySelector.Body).Members.ToList();
			else
			{
				memberExpression = lambda.Body as MemberExpression;
				members.Add(memberExpression.Member);
			}
			return members;
		}

		public static MethodInfo GetMethodInfo<T>(this Expression<Action<T>> action)
		{
			if (!(action.Body is MethodCallExpression expression))
				throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
			return expression.Method;
		}
	}

	internal static class ExpressionPropertyPathExtension
	{
		public static PropertyPath GetSimplePropertyAccess(this LambdaExpression propertyAccessExpression)
		{
			var propertyPath = MatchSimplePropertyAccess(propertyAccessExpression.Parameters.Single(),
														 propertyAccessExpression.Body);
			if (propertyPath == null)
				throw new InvalidOperationException("Invalid Property Expression");
			return propertyPath;
		}

		public static PropertyPath GetComplexPropertyAccess(this LambdaExpression propertyAccessExpression)
		{
			var propertyPath =
				((propertyAccessExpression.Parameters).Single()).MatchComplexPropertyAccess(
					propertyAccessExpression.Body);
			if (propertyPath == null)
				throw new InvalidOperationException("Invalid Complex Property Expression");
			return propertyPath;
		}

		public static IEnumerable<PropertyPath> GetSimplePropertyAccessList(this LambdaExpression propertyAccessExpression)
		{
			var enumerable = propertyAccessExpression.MatchPropertyAccessList(((p, e) => e.MatchSimplePropertyAccess(p)));
			if (enumerable == null)
				throw new InvalidOperationException("Invalid Properties Expression");
			return enumerable;
		}

		public static IEnumerable<PropertyPath> GetComplexPropertyAccessList(
			this LambdaExpression propertyAccessExpression)
		{
			var enumerable = propertyAccessExpression.MatchPropertyAccessList(((p, e) => e.MatchComplexPropertyAccess(p)));
			if (enumerable == null)
				throw new InvalidOperationException("Invalid Complex Properties Expression");
			return enumerable;
		}

		private static IEnumerable<PropertyPath> MatchPropertyAccessList(this LambdaExpression lambdaExpression,
																		 Func<Expression, Expression, PropertyPath>
																			 propertyMatcher)
		{
			if (lambdaExpression.Body.RemoveConvert() is NewExpression newExpression)
			{
				var parameterExpression = (lambdaExpression.Parameters).Single();
				var enumerable =
					(newExpression.Arguments).Select((a => propertyMatcher(a, parameterExpression))).Where(
						(p => p != (PropertyPath)null));
				if (enumerable.Count() == (newExpression.Arguments).Count())
				{
					if (!newExpression.HasDefaultMembersOnly(enumerable))
						return null;
					else
						return enumerable;
				}
			}
			var t = propertyMatcher(lambdaExpression.Body, (lambdaExpression.Parameters).Single());
			if (!(t != null))
				return null;
			return AsEnumerable(t);
		}

		private static IEnumerable<T> AsEnumerable<T>(this T t) where T : class
		{
			if (t == null)
				return Enumerable.Empty<T>();
			return new[]
					   {
						   t
					   };
		}

		private static bool HasDefaultMembersOnly(this NewExpression newExpression,
												  IEnumerable<PropertyPath> propertyPaths) => !(newExpression.Members).Where(
					((t, i) =>
					 !string.Equals(t.Name, (propertyPaths.ElementAt(i)).Last().Name, StringComparison.Ordinal))).Any();

		private static PropertyPath MatchSimplePropertyAccess(this Expression parameterExpression,
															  Expression propertyAccessExpression)
		{
			var propertyPath = MatchPropertyAccess(parameterExpression, propertyAccessExpression);
			return propertyPath.Count() != 1
					   ? null
					   : propertyPath;
		}

		private static PropertyPath MatchComplexPropertyAccess(this Expression parameterExpression,
															   Expression propertyAccessExpression)
		{
			var propertyPath = parameterExpression.MatchPropertyAccess(propertyAccessExpression);
			return !(propertyPath).Any() ? null : propertyPath;
		}

		private static PropertyPath MatchPropertyAccess(this Expression parameterExpression,
														Expression propertyAccessExpression)
		{
			var list = new List<PropertyInfo>();
			MemberExpression memberExpression;
			do
			{
				memberExpression = propertyAccessExpression.RemoveConvert() as MemberExpression;
				if (memberExpression == null)
					return PropertyPath.Empty;
				var propertyInfo = memberExpression.Member as PropertyInfo;
				if (propertyInfo == null)
					return PropertyPath.Empty;
				list.Insert(0, propertyInfo);
				propertyAccessExpression = memberExpression.Expression;
			} while (memberExpression.Expression != parameterExpression);
			return new PropertyPath(list);
		}

		public static Expression RemoveConvert(this Expression expression)
		{
			while (expression != null &&
				   (expression.NodeType == ExpressionType.Convert ||
					expression.NodeType == ExpressionType.ConvertChecked))
				expression = ((UnaryExpression)expression).Operand.RemoveConvert();
			return expression;
		}
	}
}