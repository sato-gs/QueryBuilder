// <copyright file="QueryBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.QueryBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using QueryBuilder.Queries;

    /// <summary>
    /// Represents a query builder that builds queries used in lambda expressions.
    /// </summary>
    /// <typeparam name="TEntity">The generic type parameter.</typeparam>
    public class QueryBuilder<TEntity>
    {
        private readonly ParameterExpression _parameter;
        private Query<TEntity> _query;
        private Query<TEntity> _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder{TEntity}"/> class.
        /// </summary>
        public QueryBuilder()
            : this(typeof(TEntity).Name.Substring(0, 1).ToLower())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder{TEntity}"/> class.
        /// </summary>
        /// <param name="parameter">The name of the parameter to be used.</param>
        public QueryBuilder(string parameter)
        {
            this._parameter = Expression.Parameter(typeof(TEntity), parameter);
        }

        /// <summary>
        /// Appends a query statement representing the startinig bracket (e.g. '(' of (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> Start()
        {
            if (this._query == null)
            {
                this._query = new Query<TEntity>(null, QueryType.QueryGroup);
                this._current = this._query;
            }
            else
            {
                this._current.Children.Add(new Query<TEntity>(this._current, QueryType.QueryGroup));
                this._current = this._current.Children.Last();
            }

            return this;
        }

        /// <summary>
        /// Appends a query statement representing the ending bracket (e.g. ')' of (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> End()
        {
            this._current = this._current.Parent;
            return this;
        }

        /// <summary>
        /// Appends a query statement representing &&.
        /// </summary>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> AndAlso()
        {
            this._current.Children.Last().SetQueryConnection(QueryConnection.AndAlso);
            return this;
        }

        /// <summary>
        /// Appends a query statement representing ||.
        /// </summary>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> OrElse()
        {
            this._current.Children.Last().SetQueryConnection(QueryConnection.OrElse);
            return this;
        }

        /// <summary>
        /// Appends a query statement representing a single operation involving a source property.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="operation">The operation to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> By(string sourceProperty, QueryOperation operation)
        {
            if (this._current.Type == QueryType.QueryGroup)
            {
                this._current.Children.Add(new Query<TEntity>(
                    this._current,
                    QueryType.QueryStatement,
                    operation,
                    this._parameter,
                    sourceProperty));
            }

            return this;
        }

        /// <summary>
        /// Appends a query statement representing a single operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <param name="operation">The operation to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> By(string sourceProperty, object targetValue, QueryOperation operation)
        {
            if (this._current.Type == QueryType.QueryGroup)
            {
                this._current.Children.Add(new Query<TEntity>(
                    this._current,
                    QueryType.QueryStatement,
                    operation,
                    this._parameter,
                    sourceProperty,
                    targetValue));
            }

            return this;
        }

        /// <summary>
        /// Appends a query statement representing a single operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <param name="operation">The operation to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> By(object sourceValue, object targetValue, QueryOperation operation)
        {
            if (this._current.Type == QueryType.QueryGroup)
            {
                this._current.Children.Add(new Query<TEntity>(
                    this._current,
                    QueryType.QueryStatement,
                    operation,
                    sourceValue,
                    targetValue));
            }

            return this;
        }

        /// <summary>
        /// Appends a query statement representing the Contains operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> Contains(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.Contains);
        }

        /// <summary>
        /// Appends a query statement representing the Contains operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> Contains(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.Contains);
        }

        /// <summary>
        /// Appends a query statement representing the EndsWith operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> EndsWith(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.EndsWith);
        }

        /// <summary>
        /// Appends a query statement representing the EndsWith operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> EndsWith(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.EndsWith);
        }

        /// <summary>
        /// Appends a query statement representing the Equal operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> Equal(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.Equal);
        }

        /// <summary>
        /// Appends a query statement representing the Equal operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> Equal(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.Equal);
        }

        /// <summary>
        /// Appends a query statement representing the GreaterThan operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> GreaterThan(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.GreaterThan);
        }

        /// <summary>
        /// Appends a query statement representing the GreaterThan operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> GreaterThan(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.GreaterThan);
        }

        /// <summary>
        /// Appends a query statement representing the GreaterThanOrEqual operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> GreaterThanOrEqual(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.GreaterThanOrEqual);
        }

        /// <summary>
        /// Appends a query statement representing the GreaterThanOrEqual operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> GreaterThanOrEqual(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.GreaterThanOrEqual);
        }

        /// <summary>
        /// Appends a query statement representing the LessThan operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> LessThan(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.LessThan);
        }

        /// <summary>
        /// Appends a query statement representing the LessThan operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> LessThan(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.LessThan);
        }

        /// <summary>
        /// Appends a query statement representing the LessThanOrEqual operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> LessThanOrEqual(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.LessThanOrEqual);
        }

        /// <summary>
        /// Appends a query statement representing the LessThanOrEqual operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> LessThanOrEqual(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.LessThanOrEqual);
        }

        /// <summary>
        /// Appends a query statement representing the None operation involving a source property.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> None(string sourceProperty)
        {
            return this.By(sourceProperty, QueryOperation.None);
        }

        /// <summary>
        /// Appends a query statement representing the Not Contains operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotContains(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.NotContains);
        }

        /// <summary>
        /// Appends a query statement representing the Not Contains operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotContains(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.NotContains);
        }

        /// <summary>
        /// Appends a query statement representing the Not EndsWith operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotEndsWith(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.NotEndsWith);
        }

        /// <summary>
        /// Appends a query statement representing the Not EndsWith operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotEndsWith(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.NotEndsWith);
        }

        /// <summary>
        /// Appends a query statement representing the Not Equal operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotEqual(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.NotEqual);
        }

        /// <summary>
        /// Appends a query statement representing the Not Equal operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotEqual(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.NotEqual);
        }

        /// <summary>
        /// Appends a query statement representing the Not StartsWith operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotStartsWith(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.NotStartsWith);
        }

        /// <summary>
        /// Appends a query statement representing the Not StartsWith operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> NotStartsWith(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.NotStartsWith);
        }

        /// <summary>
        /// Appends a query statement representing the StartsWith operation involving a source property and a target value.
        /// </summary>
        /// <param name="sourceProperty">The source property to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> StartsWith(string sourceProperty, object targetValue)
        {
            return this.By(sourceProperty, targetValue, QueryOperation.StartsWith);
        }

        /// <summary>
        /// Appends a query statement representing the StartsWith operation involving a source value and a target value.
        /// </summary>
        /// <param name="sourceValue">The source value to be used.</param>
        /// <param name="targetValue">The target value to be used.</param>
        /// <returns>The query builder object itself to chain methods afterwards.</returns>
        public QueryBuilder<TEntity> StartsWith(object sourceValue, object targetValue)
        {
            return this.By(sourceValue, targetValue, QueryOperation.StartsWith);
        }

        /// <summary>
        /// Builds all queries accumulated.
        /// </summary>
        /// <returns>The expression representing the whole lambda expressions.</returns>
        public Expression<Func<TEntity, bool>> Build()
        {
            return this.Build<bool>();
        }

        /// <summary>
        /// Builds all queries accumulated.
        /// </summary>
        /// <typeparam name="TPropertyType">The generic type parameter.</typeparam>
        /// <returns>The expression representing the whole lambda expressions.</returns>
        public Expression<Func<TEntity, TPropertyType>> Build<TPropertyType>()
        {
            var body = this._current == null ? this.BuildQueries(this._query.Children) : Expression.Empty();
            return Expression.Lambda<Func<TEntity, TPropertyType>>(body, this._parameter);
        }

        /// <summary>
        /// Builds all queries accumulated by connecting single queries.
        /// </summary>
        /// <param name="query">The query to be used.</param>
        /// <returns>The expression representing the whole lambda expressions.</returns>
        private Expression BuildQueries(List<Query<TEntity>> query)
        {
            Expression body = null;
            QueryConnection connection = QueryConnection.None;

            foreach (var q in query)
            {
                if (q.Type == QueryType.QueryGroup)
                {
                    switch (connection)
                    {
                        // This is when i == 0
                        case QueryConnection.None:
                            body = this.BuildQueries(q.Children);
                            break;
                        case QueryConnection.AndAlso:
                            body = Expression.AndAlso(body, this.BuildQueries(q.Children));
                            break;
                        case QueryConnection.OrElse:
                            body = Expression.OrElse(body, this.BuildQueries(q.Children));
                            break;
                    }
                }
                else if (q.Type == QueryType.QueryStatement)
                {
                    switch (connection)
                    {
                        // This is when i == 0
                        case QueryConnection.None:
                            body = q.BuildQuery();
                            break;
                        case QueryConnection.AndAlso:
                            body = Expression.AndAlso(body, q.BuildQuery());
                            break;
                        case QueryConnection.OrElse:
                            body = Expression.OrElse(body, q.BuildQuery());
                            break;
                    }
                }

                connection = q.Connection;
            }

            return body;
        }
    }
}