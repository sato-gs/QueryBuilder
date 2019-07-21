using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QueryBuilder.QueryOperations
{
    public class EndsWith : QueryOperationBase
    {
        public override Expression BuildExpression(MemberExpression member, Expression value)
        {
            MethodInfo methodInfo = value.Type.GetMethod(MethodName, new Type[] { value.Type });
            return Expression.Call(member, methodInfo, value);
        }
    }
}
