// <copyright file="QueryOperationBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents the base of all query operations.
    /// </summary>
    public abstract class QueryOperationBase
    {
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string MethodName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Builds a member & value expression for inheriting query operation classes.
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public abstract Expression BuildExpression(Expression member, Expression value);
    }
}