using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Queries
{
    public enum QueryOperation
    {
        Contains,
        EndsWith,
        Equal,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        None,
        NotContains,
        NotEndsWith,
        NotEqual,
        NotStartsWith,
        StartsWith
    }
}
