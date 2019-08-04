// <copyright file="EndsWith.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Represents .EndsWith().
    /// </summary>
    public class EndsWith : QueryOperationBase
    {
        /// <summary>
        /// Builds a member & value expression for .EndsWith() (e.g. p.Name.EndsWith("Product 1") or "Product 10".EndsWith("Product 1") of (p => p.Name.EndsWith("Product 1")) or ("Product 10".EndsWith("Product 1"))).
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public override Expression BuildExpression(Expression member, Expression value)
        {
            MethodInfo methodInfo = value.Type.GetMethod(this.MethodName, new Type[] { value.Type });
            return Expression.Call(member, methodInfo, value);
        }
    }
}
