using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QueryBuilder.QueryOperations
{
    public class Contains : QueryOperationBase
    {
        public override Expression BuildExpression(MemberExpression member, Expression value)
        {
            MethodInfo methodInfo;
            if (value.GetType() == typeof(ConstantExpression))
            {
                methodInfo = member.Type.GetMethod(MethodName, new Type[] { member.Type });
                return Expression.Call(member, methodInfo, value);
            }
            else
            {
                methodInfo = typeof(Enumerable).GetMethods()
                                .Single(m => m.Name == MethodName
                                        && m.IsGenericMethod
                                        && m.IsGenericMethodDefinition
                                        && m.GetParameters().Length == 2).MakeGenericMethod(member.Type);
                return Expression.Call(methodInfo, value, member);
            }
        }
    }
}
