// <copyright file="None.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents no operation.
    /// </summary>
    public class None : QueryOperationBase
    {
        /// <summary>
        /// Builds a member & value expression for no operation.
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public override Expression BuildExpression(Expression member, Expression value)
        {
            return member;
        }
    }
}
