using Sketch7.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	public static class QueryableExtensions
	{
		#region WhereIn / WhereNotIn
		public static IQueryable<T> WhereNotIn<T, TValue>(this IQueryable<T> source,
														  Expression<Func<T, TValue>> propertySelector, params TValue[] values)
			=> source.Where(GetWhereInExpression(propertySelector, values, false));

		public static IQueryable<T> WhereNotIn<T, TValue>(this IQueryable<T> source,
														  Expression<Func<T, TValue>> propertySelector,
														  ICollection<TValue> values)
			=> source.Where(GetWhereInExpression(propertySelector, values, false));

		public static IQueryable<T> WhereIn<T, TValue>(this IQueryable<T> source, Expression<Func<T, TValue>> propertySelector,
													   params TValue[] values)
			=> source.Where(GetWhereInExpression(propertySelector, values));

		public static IQueryable<T> WhereIn<T, TValue>(this IQueryable<T> source, Expression<Func<T, TValue>> propertySelector,
													   ICollection<TValue> values)
			=> source.Where(GetWhereInExpression(propertySelector, values));

		private static Expression<Func<T, bool>> GetWhereInExpression<T, TValue>(Expression<Func<T, TValue>> propertySelector,
																				 ICollection<TValue> values,
																				 bool isEquals = true)
		{
			var p = propertySelector.Parameters.Single();
			if (!values.Any())
				return e => false;

			var equals = isEquals
							 ? values.Select(
								 value =>
								 (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))))
							 : values.Select(
								 value =>
								 (Expression)Expression.NotEqual(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
			var body = equals.Aggregate(Expression.Or);

			return Expression.Lambda<Func<T, bool>>(body, p);
		}
		#endregion  WhereIn / WhereNotIn

		/// <summary>
		/// 	Filters a sequence of values based on a predicate.
		/// </summary>
		/// <typeparam name="T"> The entity type. </typeparam>
		/// <typeparam name="TValue"> The key selector type. </typeparam>
		/// <param name="source"> Source query. </param>
		/// <param name="propertySelector"> A property to filter by. </param>
		/// <param name="value"> Property value to filter. </param>
		/// <param name="operation"> Operation to perform. (Equal=Default) </param>
		/// <returns> Returns an IQueryable </returns>
		public static IQueryable<T> Where<T, TValue>(this IQueryable<T> source, Expression<Func<T, TValue>> propertySelector,
													 TValue value, WhereOperation operation = WhereOperation.Equal)
		{
			var p = propertySelector.Parameters.Single();
			BinaryExpression expOperation;
			switch (operation)
			{
				case WhereOperation.Equal:
					expOperation = Expression.Equal(propertySelector.Body, Expression.Constant(value, propertySelector.ReturnType));
					break;
				case WhereOperation.NotEqual:
					expOperation = Expression.NotEqual(propertySelector.Body, Expression.Constant(value, propertySelector.ReturnType));
					break;
				case WhereOperation.GreaterThan:
					expOperation = Expression.GreaterThan(propertySelector.Body,
														  Expression.Constant(value, propertySelector.ReturnType));
					break;
				case WhereOperation.GreaterThanOrEqual:
					expOperation = Expression.GreaterThanOrEqual(propertySelector.Body,
																 Expression.Constant(value, propertySelector.ReturnType));
					break;
				case WhereOperation.LessThan:
					expOperation = Expression.LessThan(propertySelector.Body, Expression.Constant(value, propertySelector.ReturnType));
					break;
				case WhereOperation.LessThanOrEqual:
					expOperation = Expression.LessThanOrEqual(propertySelector.Body,
															  Expression.Constant(value, propertySelector.ReturnType));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(operation));
			}

			var predicate = Expression.Lambda<Func<T, bool>>(expOperation, p);

			return source.Where(predicate);
		}

		#region WhereComposite

		public static IQueryable<T> WhereCompositeNotEqual<T, TValue>(this IQueryable<T> source,
																	  Expression<Func<T, TValue>> propertySelector,
																	  params TValue[][] values)
			=> source.WhereCompositeNotEqual(propertySelector, null, values);

		public static IQueryable<T> WhereCompositeNotEqual<T, TValue>(this IQueryable<T> source,
																	  Expression<Func<T, TValue>> propertySelector,
																	  IEnumerable<MemberInfo> memberInfoList,
																	  params TValue[][] values)
			=> source.Where(WhereCompositeBuilder(propertySelector, memberInfoList, values, false));

		public static IQueryable<T> WhereCompositeNotEqual<T, TValue>(this IQueryable<T> source,
																	  IEnumerable<MemberInfo> memberInfoList,
																	  params TValue[][] values)
			=> source.Where(GetWhereCompositeExpression<T, TValue>(memberInfoList, values, false));

		public static IQueryable<T> WhereComposite<T, TValue>(this IQueryable<T> source,
															  Expression<Func<T, TValue>> propertySelector,
															  params TValue[][] values)
			=> source.WhereComposite(propertySelector, null, values);

		public static IQueryable<T> WhereComposite<T, TValue>(this IQueryable<T> source,
															  Expression<Func<T, TValue>> propertySelector,
															  IEnumerable<MemberInfo> memberInfoList, params TValue[][] values)
			=> source.Where(WhereCompositeBuilder(propertySelector, memberInfoList, values));

		public static IQueryable<T> WhereComposite<T, TValue>(this IQueryable<T> source,
															  IEnumerable<MemberInfo> memberInfoList, params TValue[][] values)
			=> source.Where(GetWhereCompositeExpression<T, TValue>(memberInfoList, values));

		private static Expression<Func<T, bool>> WhereCompositeBuilder<T, TValue>(
			Expression<Func<T, TValue>> propertySelector, IEnumerable<MemberInfo> memberInfoList, ICollection<TValue[]> values,
			bool isEquals = true)
		{
			var members = propertySelector.ExtractMembers();

			if (memberInfoList != null)
				members = members.Concat(memberInfoList).Distinct(x => x.Name).ToList();

			return GetWhereCompositeExpression<T, TValue>(members, values, isEquals);
		}

		private static Expression<Func<T, bool>> GetWhereCompositeExpression<T, TValue>(
			IEnumerable<MemberInfo> memberInfoList, ICollection<TValue[]> values, bool isEquals = true)
		{
			var parameter = Expression.Parameter(typeof(T), "e");
			if (!values.Any())
				return e => false;

			var expressionMembers =
				memberInfoList.Select(memberInfo => Expression.Property(parameter, memberInfo.Name)).Cast<Expression>().ToList();

			var firstValueRow = values.First();
			if (firstValueRow.Length != expressionMembers.Count)
				throw new InvalidOperationException("Expression Members and values count do not match.");

			var allExpressions = new List<Expression>();
			foreach (var value in values)
			{
				var rowExpressions = (isEquals)
										 ? expressionMembers.Select(
											 (t, i) => (Expression)Expression.Equal(t, Expression.Constant(value[i], value[i].GetType())))
											   .ToList()
										 : expressionMembers.Select(
											 (t, i) =>
											 (Expression)Expression.NotEqual(t, Expression.Constant(value[i], value[i].GetType())))
											   .ToList();
				var rowBody = rowExpressions.Aggregate(Expression.And);
				allExpressions.Add(rowBody);
			}
			var fullBody = allExpressions.Aggregate(Expression.Or);
			//E.G - WHERE (ClientApplicationId = 1 and CountryId = 2) OR (ClientApplicationId = 2 and CountryId = 1)
			return Expression.Lambda<Func<T, bool>>(fullBody, parameter);
		}

		#endregion

		public static IQueryable<T> Paging<T>(this IQueryable<T> source, int pageIndex, int pageSize)
		{
			if (pageIndex < 1) pageIndex = 1;
			if (pageSize < 1) pageSize = 10;

			return source.Skip((pageIndex - 1) * pageSize);
		}
	}
}