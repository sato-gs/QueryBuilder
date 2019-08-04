// <copyright file="Equal.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents ==.
    /// </summary>
    public class Equal : QueryOperationBase
    {
        /// <summary>
        /// Builds a member & value expression for == (e.g. p.ProductId == 1 or 10 == 1 of (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public override Expression BuildExpression(Expression member, Expression value)
        {
            return Expression.Equal(member, value);
        }
    }
}
