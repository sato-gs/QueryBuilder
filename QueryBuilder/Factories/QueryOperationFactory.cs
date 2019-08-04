// <copyright file="QueryOperationFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.Factories
{
    using QueryBuilder.Queries;
    using QueryBuilder.QueryOperations;

    /// <summary>
    /// Represents a factory class used in the <see cref="Query{TEntity}"/> class.
    /// </summary>
    public class QueryOperationFactory
    {
        /// <summary>
        /// Creates a new instance of the query operation class based on the given <see cref="QueryOperation"/> enum.
        /// </summary>
        /// <param name="operation">The query operation to be selected.</param>
        /// <returns>The new instance of the query operation class based on the given <see cref="QueryOperation"/> enum.</returns>
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
