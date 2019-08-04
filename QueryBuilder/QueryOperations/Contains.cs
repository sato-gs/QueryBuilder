// <copyright file="Contains.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryOperations
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using QueryBuilder.Extensions;

    /// <summary>
    /// Represents .Contains().
    /// </summary>
    public class Contains : QueryOperationBase
    {
        /// <summary>
        /// Builds a member & value expression for .Contains() (e.g. p.ProductIds.Contains(new int[] {1, 2, 3}) or 10.Contains(new int[] {1, 2, 3}) of (p => p.ProductIds.Contains(new int[] {1, 2, 3})) or (10.Contains(new int[] {1, 2, 3}))).
        /// </summary>
        /// <param name="member">The member expression to be used.</param>
        /// <param name="value">The value expression to be used.</param>
        /// <returns>The expression representing the member & value part of lambda expressions.</returns>
        public override Expression BuildExpression(Expression member, Expression value)
        {
            MethodInfo methodInfo;
            if (!member.Type.IsTypeEnumerable() && !value.Type.IsTypeEnumerable())
            {
                methodInfo = member.Type.GetMethod(this.MethodName, new Type[] { member.Type });
                return Expression.Call(member, methodInfo, value);
            }
            else
            {
                methodInfo = typeof(Enumerable).GetMethods()
                                .Single(m => m.Name == this.MethodName
                                        && m.IsGenericMethod
                                        && m.IsGenericMethodDefinition
                                        && m.GetParameters().Length == 2)
                                .MakeGenericMethod(member.Type.GetNonEnumerableBaseType());
                return Expression.Call(methodInfo, member, value);
            }
        }
    }
}
