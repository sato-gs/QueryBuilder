// <copyright file="NotEndsWith.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents !.EndsWith().
    /// </summary>
    public class NotEndsWith : QueryOperationBase
    {
        /// <summary>
        /// Builds a member & value expression for !.EndsWith() (e.g. !p.Name.EndsWith("Product 1") or !"Product 10".EndsWith("Product 1") of (p => !p.Name.EndsWith("Product 1")) or (!"Product 10".EndsWith("Product 1"))).
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public override Expression BuildExpression(Expression member, Expression value)
        {
            return Expression.Not(new EndsWith().BuildExpression(member, value));
        }
    }
}
