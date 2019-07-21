using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QueryBuilder.QueryOperations
{
    public class NotEqual : QueryOperationBase
    {
        public override Expression BuildExpression(MemberExpression member, Expression value)
        {
            return Expression.NotEqual(member, value);
        }
    }
}
