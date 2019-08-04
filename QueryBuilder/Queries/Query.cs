// <copyright file="Query.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using QueryBuilder.Extensions;
    using QueryBuilder.Factories;

    /// <summary>
    /// Represents a single query in lambda expressions (e.g. (p => p.ProductId == 1) or (10 == 1)).
    /// </summary>
    /// <typeparam name="TEntity">The generic type parameter.</typeparam>
    public class Query<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TEntity}"/> class to produce a parent query.
        /// </summary>
        /// <param name="parent">The parent query.</param>
        /// <param name="type">The query type.</param>
        public Query(
            Query<TEntity> parent,
            QueryType type)
        {
            this.Parent = parent;
            this.Children = new List<Query<TEntity>>();
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TEntity}"/> class to produce a child query involving a source property (e.g. (p => p.PropertyId)).
        /// </summary>
        /// <param name="parent">The parent query.</param>
        /// <param name="type">The query type.</param>
        /// <param name="operation">The query operation.</param>
        /// <param name="parameter">The parameter expression.</param>
        /// <param name="sourceProperty">The source property.</param>
        public Query(
            Query<TEntity> parent,
            QueryType type,
            QueryOperation operation,
            ParameterExpression parameter,
            string sourceProperty)
        {
            this.Parent = parent;
            this.Children = new List<Query<TEntity>>();
            this.Type = type;
            this.Operation = operation;
            this.ParameterExpression = parameter;
            this.SourceProperty = sourceProperty;
            this.IsSourceFirst = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TEntity}"/> class to produce a child query involving a source property (first) and a target value (e.g. (p => p.PropertyId == 1)).
        /// </summary>
        /// <param name="parent">The parent query.</param>
        /// <param name="type">The query type.</param>
        /// <param name="operation">The query operation.</param>
        /// <param name="parameter">The parameter expression.</param>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetValue">The target value.</param>
        public Query(
            Query<TEntity> parent,
            QueryType type,
            QueryOperation operation,
            ParameterExpression parameter,
            string sourceProperty,
            object targetValue)
        {
            this.Parent = parent;
            this.Children = new List<Query<TEntity>>();
            this.Type = type;
            this.Operation = operation;
            this.ParameterExpression = parameter;
            this.SourceProperty = sourceProperty;
            this.IsOriginalValueNull = targetValue == null;
            this.TargetValue = this.ValidateValue(targetValue, this.PropertyNonEnumerableType);
            this.IsSourceFirst = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TEntity}"/> class to produce a child query involving a target value (first) and a source property (e.g. (p => 1 == p.ProductId)).
        /// </summary>
        /// <param name="parent">The parent query.</param>
        /// <param name="type">The query type.</param>
        /// <param name="operation">The query operation.</param>
        /// <param name="parameter">The parameter expression.</param>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetValue">The target value.</param>
        public Query(
            Query<TEntity> parent,
            QueryType type,
            QueryOperation operation,
            ParameterExpression parameter,
            object targetValue,
            string sourceProperty)
        {
            this.Parent = parent;
            this.Children = new List<Query<TEntity>>();
            this.Type = type;
            this.Operation = operation;
            this.ParameterExpression = parameter;
            this.SourceProperty = sourceProperty;
            this.IsOriginalValueNull = targetValue == null;
            this.TargetValue = this.ValidateValue(targetValue, this.PropertyNonEnumerableType);
            this.IsSourceFirst = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TEntity}"/> class to produce a child query involving a source value (first) and a target value (e.g. (10 == 1)).
        /// </summary>
        /// <param name="parent">The parent query.</param>
        /// <param name="type">The query type.</param>
        /// <param name="operation">The query operation.</param>
        /// <param name="sourceValue">The source value.</param>
        /// <param name="targetValue">The target value.</param>
        public Query(
            Query<TEntity> parent,
            QueryType type,
            QueryOperation operation,
            object sourceValue,
            object targetValue)
        {
            this.Parent = parent;
            this.Children = new List<Query<TEntity>>();
            this.Type = type;
            this.Operation = operation;
            this.ParameterExpression = null;
            this.SourceValue = sourceValue;
            this.TargetValue = targetValue;
            this.IsSourceFirst = true;
        }

        /// <summary>
        /// Gets the parent query.
        /// </summary>
        public Query<TEntity> Parent { get; }

        /// <summary>
        /// Gets or sets the child query.
        /// </summary>
        public List<Query<TEntity>> Children { get; set; }

        /// <summary>
        /// Gets the <see cref="QueryType"/> enum.
        /// </summary>
        public QueryType Type { get; } = QueryType.QueryGroup;

        /// <summary>
        /// Gets the <see cref="QueryOperation"/> enum.
        /// </summary>
        public QueryOperation Operation { get; } = QueryOperation.None;

        /// <summary>
        /// Gets or sets the <see cref="QueryConnection"/> enum.
        /// </summary>
        public QueryConnection Connection { get; set; } = QueryConnection.None;

        /// <summary>
        /// Gets the property that acts as a source (e.g. ProductId of (p => p.ProductId == 1)).
        /// </summary>
        public string SourceProperty { get; }

        /// <summary>
        /// Gets the value that acts as a source (e.g. 10 of (10 == 1)).
        /// </summary>
        public object SourceValue { get; }

        /// <summary>
        /// Gets the value that acts as a target (e.g. 1 of (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        public object TargetValue { get; }

        /// <summary>
        /// Gets a value indicating whether or not the target value is originally null.
        /// </summary>
        public bool IsOriginalValueNull { get; }

        /// <summary>
        /// Gets a value indicating whether or not the source property/value comes first (e.g. (p => p.ProductId == 1)/(10 == 1) if true, but (p => 1 == p.ProductId)/(1 == 10) if false).
        /// </summary>
        public bool IsSourceFirst { get; }

        /// <summary>
        /// Gets the parameter expression representing the parameter part of lambda expressions (e.g. p of (p => p.ProductId == 1)).
        /// </summary>
        public ParameterExpression ParameterExpression { get; }

        /// <summary>
        /// Gets the member expression representing the member part of lambda expressions (e.g. p.ProductId or 10 of (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        public Expression MemberExpression
        {
            get
            {
                if (this.ParameterExpression != null)
                {
                    return this.SourceProperty.Split('.').Aggregate(this.ParameterExpression as Expression, Expression.Property);
                }
                else
                {
                    return this.BuildValueExpression(this.SourceValue, this.ValueType);
                }
            }
        }

        /// <summary>
        /// Gets the value expression representing the value part of lambda expressions (e.g. 1 of (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        public Expression ValueExpression
        {
            get
            {
                if (this.ParameterExpression != null)
                {
                    return this.BuildValueExpression(this.TargetValue, this.PropertyType);
                }
                else
                {
                    return this.BuildValueExpression(this.TargetValue, this.ValueType);
                }
            }
        }

        /// <summary>
        /// Gets the type of the property used in lambda expressions.
        /// </summary>
        public Type PropertyType
        {
            get
            {
                return ((this.MemberExpression as MemberExpression).Member as PropertyInfo).PropertyType;
            }
        }

        /// <summary>
        /// Gets the non-Enumerable type of the property used in lambda expressions.
        /// </summary>
        public Type PropertyNonEnumerableType
        {
            get
            {
                return this.PropertyType.GetNonEnumerableBaseType();
            }
        }

        /// <summary>
        /// Gets the type of the value used in lambda expressions.
        /// </summary>
        public Type ValueType
        {
            get
            {
                return new List<Type>() { this.SourceValue?.GetType(), this.TargetValue?.GetType() }.FirstOrDefault(t => t != null).GetNonEnumerableBaseType().ConvertToNullableType();
            }
        }

        /// <summary>
        /// Sets the query connection.
        /// </summary>
        /// <param name="connection">The query connection to be set.</param>
        public void SetQueryConnection(QueryConnection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Builds a value expression (e.g. p.ProductId == 1 or 10 == 1 of (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        /// <param name="value">The value to be used.</param>
        /// <param name="type">The type to be used.</param>
        /// <returns>The expression representing the value part of lambda expressions.</returns>
        public Expression BuildValueExpression(object value, Type type)
        {
            if (value?.GetType().IsTypeEnumerable() == true)
            {
                var expressions = new List<Expression>();
                foreach (var val in (System.Collections.IEnumerable)value)
                {
                    expressions.Add(Expression.Convert(Expression.Constant(val), type) as Expression);
                }

                return Expression.NewArrayInit(type, expressions);
            }
            else
            {
                return type == null ? Expression.Constant(value) : Expression.Convert(Expression.Constant(value), type) as Expression;
            }
        }

        /// <summary>
        /// Builds a query (e.g. (p => p.ProductId == 1) or (10 == 1)).
        /// </summary>
        /// <returns>The expression representing the whole part of lambda expressions.</returns>
        public Expression BuildQuery()
        {
            this.ValidateQuery();
            return QueryOperationFactory
                    .Create(this.Operation)
                    .BuildExpression(this.IsSourceFirst ? this.MemberExpression : this.ValueExpression, this.IsSourceFirst ? this.ValueExpression : this.MemberExpression);
        }

        /// <summary>
        /// Validates a target value.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        /// <param name="type">The type against which the value is validated.</param>
        /// <returns>The same value/null if valid/invalid.</returns>
        private object ValidateValue(object value, Type type)
        {
            if (value?.GetType().IsTypeEnumerable() == true)
            {
                return value?.GetType().GetNonEnumerableBaseType() == type ? value : null;
            }
            else
            {
                return value?.GetType().GetNonNullableBaseType() == type.GetNonNullableBaseType() ? value : null;
            }
        }

        /// <summary>
        /// Validates a query, and throws the <see cref="ArgumentException"/> exception if invalid.
        /// </summary>
        private void ValidateQuery()
        {
            var errors = new List<string>();

            if (this.Type == QueryType.QueryGroup)
            {
                errors.Add("Invalid query type.");
            }
            else
            {
                if (this.Operation != QueryOperation.None)
                {
                    if (this.ParameterExpression != null)
                    {
                        if (this.ParameterExpression.Type != typeof(TEntity))
                        {
                            errors.Add($"Invalid parameter type.");
                        }

                        if (this.PropertyType == null)
                        {
                            errors.Add($"Invalid property name.");
                        }
                        else if (!(this.IsOriginalValueNull && this.PropertyType.IsTypeNullable()) && this.TargetValue == null)
                        {
                            errors.Add($"Invalid property value.");
                        }
                    }
                    else
                    {
                        if (this.SourceValue?.GetType().GetNonEnumerableNonNullableBaseType() != this.TargetValue?.GetType().GetNonEnumerableNonNullableBaseType() && !(this.SourceValue == null ^ this.TargetValue == null))
                        {
                            errors.Add($"Invalid value.");
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join(" & ", errors));
            }
        }
    }
}