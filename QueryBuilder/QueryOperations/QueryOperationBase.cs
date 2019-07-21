using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace QueryBuilder.QueryOperations
{
    public abstract class QueryOperationBase
    {
        public string MethodName {
            get
            {
                return this.GetType().Name;
            }
        }

        public abstract Expression BuildExpression(MemberExpression member, Expression value);
    }
}
