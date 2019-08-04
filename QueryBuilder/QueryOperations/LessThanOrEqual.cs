// <copyright file="LessThanOrEqual.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents &le;.
    /// </summary>
    public class LessThanOrEqual : QueryOperationBase
    {
        /// <summary>
        /// Builds a member & value expression for &le; (e.g. p.ProductId &le; 1 or 10 &le; 1 of (p => p.ProductId &le; 1) or (10 &le; 1)).
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public override Expression BuildExpression(Expression member, Expression value)
        {
            return Expression.LessThanOrEqual(member, value);
        }
    }
}
