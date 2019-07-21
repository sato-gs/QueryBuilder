using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QueryBuilder.QueryOperations
{
    public class NotContains : QueryOperationBase
    {
        public override Expression BuildExpression(MemberExpression member, Expression value)
        {
            return Expression.Not(new Contains().BuildExpression(member, value));
        }
    }
}
