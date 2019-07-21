using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using QueryBuilder.Factories;

namespace QueryBuilder.Queries
{
    public class Query<TEntity>
    {
        public Query<TEntity> Parent { get; }
        public List<Query<TEntity>> Children { get; set; }
        public QueryType Type { get; } = QueryType.QueryGroup;
        public QueryOperation Operation { get; } = QueryOperation.None;
        public QueryConnection Connection { get; set; } = QueryConnection.None;
        public string PropertyName { get; }
        public object PropertyValue { get; }
        public bool IsOriginalValueNull { get; }
        public ParameterExpression ParameterExpression { get; }

        public MemberExpression MemberExpression
        {
            get
            {
                return PropertyName.Split('.').Aggregate(ParameterExpression as Expression, Expression.Property) as MemberExpression;
            }
        }

        public Expression ValueExpression
        {
            get
            {
                return IsValueNonEnumerable(PropertyValue) ? BuildValueExpression(PropertyValue, PropertyConverter) : BuildValuesExpression(PropertyValue, PropertyType, PropertyConverter);
            }
        }

        public Type PropertyType
        {
            get
            {
                return ((MemberExpression as MemberExpression).Member as PropertyInfo).PropertyType;
            }
        }

        public TypeConverter PropertyConverter
        {
            get
            {
                return TypeDescriptor.GetConverter(PropertyType);
            }
        }

        public bool IsPropertyNullable
        {
            get
            {
                return !PropertyType.IsValueType || (PropertyType.IsGenericType && PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
            }
        }

        public Query(
            Query<TEntity> parent,
            QueryType type)
        {
            Parent = parent;
            Children = new List<Query<TEntity>>();
            Type = type;
        }

        public Query(
            Query<TEntity> parent,
            QueryType type,
            QueryOperation operation,
            ParameterExpression parameter,
            string propertyName)
        {
            Parent = parent;
            Children = new List<Query<TEntity>>();
            Type = type;
            Operation = operation;
            ParameterExpression = parameter;
            PropertyName = propertyName;
        }

        public Query(
            Query<TEntity> parent,
            QueryType type,
            QueryOperation operation,
            ParameterExpression parameter,
            string propertyName,
            object propertyValue)
        {
            Parent = parent;
            Children = new List<Query<TEntity>>();
            Type = type;
            Operation = operation;
            ParameterExpression = parameter;
            PropertyName = propertyName;
            IsOriginalValueNull = propertyValue == null;
            PropertyValue = IsValueNonEnumerable(propertyValue) ? ValidateValue(propertyValue, PropertyType) : ValidateValues(propertyValue, PropertyType);
        }

        public void SetQueryConnection(QueryConnection connection)
        {
            Connection = connection;
        }

        public Expression BuildQuery()
        {
            ValidateQuery();
            return QueryOperationFactory
                    .Create(Operation)
                    .BuildExpression(MemberExpression, ValueExpression);
        }

        private void ValidateQuery()
        {
            var errors = new List<string>();

            if (Type == QueryType.QueryGroup)
            {
                errors.Add("Invalid query type.");
            }
            else
            {
                if (Operation != QueryOperation.None)
                {
                    if (ParameterExpression.Type != typeof(TEntity))
                    {
                        errors.Add($"Invalid parameter type.");
                    }

                    if (PropertyType == null)
                    {
                        errors.Add($"Invalid property name.");
                    }

                    if (!(IsOriginalValueNull && IsPropertyNullable) && PropertyValue == null)
                    {
                        errors.Add($"Invalid property value.");
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join(" & ", errors));
            }
        }

        private bool IsValueNonEnumerable(object value)
        {
            return value == null || value.GetType() == typeof(string) || !value.GetType().GetInterfaces().Contains(typeof(System.Collections.IEnumerable));
        }

        private object ValidateValue(object value, Type type)
        {
            return value?.GetType() == type ? value : null;
        }

        private object ValidateValues(object values, Type type)
        {
            foreach (var value in (System.Collections.IEnumerable)values)
            {
                if (ValidateValue(value, type) == null)
                {
                    return null;
                }
            }

            return values;
        }

        private Expression BuildValueExpression(object value, TypeConverter converter)
        {
            return Expression.Constant(value == null ? value : converter.ConvertFromInvariantString(value.ToString()));
        }

        private Expression BuildValuesExpression(object values, Type type, TypeConverter converter)
        {
            var expressions = new List<Expression>();
            foreach (var value in (System.Collections.IEnumerable)values)
            {
                expressions.Add(BuildValueExpression(value, converter));
            }

            return Expression.NewArrayInit(type, expressions);
        }
    }
}
