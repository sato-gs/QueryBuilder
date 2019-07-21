using QueryBuilder.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace QueryBuilder.QueryBuilders
{
    public class QueryBuilder<TEntity>
    {
        private readonly ParameterExpression _parameter;
        private Query<TEntity> _query;
        private Query<TEntity> _current;

        public QueryBuilder()
            : this(typeof(TEntity).Name.Substring(0, 1).ToLower())
        {
        }

        public QueryBuilder(string parameter)
        {
            _parameter = Expression.Parameter(typeof(TEntity), parameter);
        }

        public QueryBuilder<TEntity> Start()
        {
            if (_query == null)
            {
                _query = new Query<TEntity>(null, QueryType.QueryGroup);
                _current = _query;
            }
            else
            {
                _current.Children.Add(new Query<TEntity>(_current, QueryType.QueryGroup));
                _current = _current.Children.Last();
            }

            return this;
        }

        public QueryBuilder<TEntity> End()
        {
            _current = _current.Parent;
            return this;
        }

        public QueryBuilder<TEntity> AndAlso()
        {
            _current.Children.Last().SetQueryConnection(QueryConnection.AndAlso);
            return this;
        }

        public QueryBuilder<TEntity> OrElse()
        {
            _current.Children.Last().SetQueryConnection(QueryConnection.OrElse);
            return this;
        }

        public QueryBuilder<TEntity> By(string name, QueryOperation operation)
        {
            if (_current.Type == QueryType.QueryGroup)
            {
                _current.Children.Add(new Query<TEntity>(_current,
                                                        QueryType.QueryStatement,
                                                        operation,
                                                        _parameter,
                                                        name));
            }

            return this;
        }

        public QueryBuilder<TEntity> By(string name, object value, QueryOperation operation)
        {
            if (_current.Type == QueryType.QueryGroup)
            {
                _current.Children.Add(new Query<TEntity>(_current,
                                                        QueryType.QueryStatement,
                                                        operation,
                                                        _parameter,
                                                        name,
                                                        value));
            }

            return this;
        }

        public QueryBuilder<TEntity> Contains(string name, object value)
        {
            return By(name, value, QueryOperation.Contains);
        }

        public QueryBuilder<TEntity> EndsWith(string name, object value)
        {
            return By(name, value, QueryOperation.EndsWith);
        }

        public QueryBuilder<TEntity> Equal(string name, object value)
        {
            return By(name, value, QueryOperation.Equal);
        }

        public QueryBuilder<TEntity> GreaterThan(string name, object value)
        {
            return By(name, value, QueryOperation.GreaterThan);
        }

        public QueryBuilder<TEntity> GreaterThanOrEqual(string name, object value)
        {
            return By(name, value, QueryOperation.GreaterThanOrEqual);
        }

        public QueryBuilder<TEntity> LessThan(string name, object value)
        {
            return By(name, value, QueryOperation.LessThan);
        }

        public QueryBuilder<TEntity> LessThanOrEqual(string name, object value)
        {
            return By(name, value, QueryOperation.LessThanOrEqual);
        }

        public QueryBuilder<TEntity> None(string propertyName)
        {
            return By(propertyName, QueryOperation.None);
        }

        public QueryBuilder<TEntity> NotContains(string name, object value)
        {
            return By(name, value, QueryOperation.NotContains);
        }

        public QueryBuilder<TEntity> NotEndsWith(string name, object value)
        {
            return By(name, value, QueryOperation.NotEndsWith);
        }

        public QueryBuilder<TEntity> NotEqual(string name, object value)
        {
            return By(name, value, QueryOperation.NotEqual);
        }

        public QueryBuilder<TEntity> NotStartsWith(string name, object value)
        {
            return By(name, value, QueryOperation.NotStartsWith);
        }

        public QueryBuilder<TEntity> StartsWith(string name, object value)
        {
            return By(name, value, QueryOperation.StartsWith);
        }

        public Expression<Func<TEntity, bool>> Build()
        {
            return Build<bool>();
        }

        public Expression<Func<TEntity, TPropertyType>> Build<TPropertyType>()
        {
            var body = _current == null ? BuildQueries(_query.Children) : Expression.Empty();
            return Expression.Lambda<Func<TEntity, TPropertyType>>(body, _parameter);
        }

        public Expression BuildQueries(List<Query<TEntity>> query)
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
                            body = BuildQueries(q.Children);
                            break;
                        case QueryConnection.AndAlso:
                            body = Expression.AndAlso(body, BuildQueries(q.Children));
                            break;
                        case QueryConnection.OrElse:
                            body = Expression.OrElse(body, BuildQueries(q.Children));
                            break;
                    }
                }
                else if (q.Type == QueryType.QueryStatement)
                {
                    switch (connection)
                    {
                        //  This is when i == 0
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
