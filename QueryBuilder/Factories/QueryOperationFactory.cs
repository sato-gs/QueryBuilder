using QueryBuilder.Queries;
using QueryBuilder.QueryOperations;

namespace QueryBuilder.Factories
{
    public class QueryOperationFactory
    {
        public static QueryOperationBase Create(QueryOperation operation)
        {
            switch (operation)
            {
                case QueryOperation.Contains:
                    return new Contains();
                case QueryOperation.EndsWith:
                    return new EndsWith();
                case QueryOperation.Equal:
                    return new Equal();
                case QueryOperation.GreaterThan:
                    return new GreaterThan();
                case QueryOperation.GreaterThanOrEqual:
                    return new GreaterThanOrEqual();
                case QueryOperation.LessThan:
                    return new LessThan();
                case QueryOperation.LessThanOrEqual:
                    return new LessThanOrEqual();
                case QueryOperation.None:
                    return new None();
                case QueryOperation.NotContains:
                    return new NotContains();
                case QueryOperation.NotEndsWith:
                    return new NotEndsWith();
                case QueryOperation.NotEqual:
                    return new NotEqual();
                case QueryOperation.NotStartsWith:
                    return new NotStartsWith();
                case QueryOperation.StartsWith:
                    return new StartsWith();
                default:
                    return new None();
            }
        }
    }
}
