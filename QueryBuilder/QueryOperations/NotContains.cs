// <copyright file="NotContains.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents !.Contains().
    /// </summary>
    public class NotContains : QueryOperationBase
    {
        /// <summary>
        /// Builds a member & value expression for !.Contains() (e.g. !p.ProductIds.Contains(new int[] {1, 2, 3}) or !10.Contains(new int[] {1, 2, 3}) of (p => !p.ProductIds.Contains(new int[] {1, 2, 3})) or (!10.Contains(new int[] {1, 2, 3}))).
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public override Expression BuildExpression(Expression member, Expression value)
        {
            return Expression.Not(new Contains().BuildExpression(member, value));
        }
    }
}
