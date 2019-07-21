﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace QueryBuilder.QueryOperations
{
    public class None : QueryOperationBase
    {
        public override Expression BuildExpression(MemberExpression member, Expression value)
        {
            return member;
        }
    }
}
