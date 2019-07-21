using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace QueryBuilder.QueryOperations
{
    public class NotStartsWith : QueryOperationBase
    {
        public override Expression BuildExpression(MemberExpression member, Expression value)
        {
            return Expression.Not(new StartsWith().BuildExpression(member, value));
        }
    }
}
