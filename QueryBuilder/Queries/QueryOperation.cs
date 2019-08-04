// <copyright file="QueryOperation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.Queries
{
    /// <summary>
    /// Represents a query operation that underlies queries.
    /// </summary>
    public enum QueryOperation
    {
        /// <summary>
        /// Represents .Contains().
        /// </summary>
        Contains,

        /// <summary>
        /// Represents .EndsWith().
        /// </summary>
        EndsWith,

        /// <summary>
        /// Represents ==.
        /// </summary>
        Equal,

        /// <summary>
        /// Represents >.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Represents >=.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Represents <.
        /// </summary>
        LessThan,

        /// <summary>
        /// Represents <=.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Represents no operation.
        /// </summary>
        None,

        /// <summary>
        /// Represents !.Contains().
        /// </summary>
        NotContains,

        /// <summary>
        /// Represents !.EndsWith().
        /// </summary>
        NotEndsWith,

        /// <summary>
        /// Represents !=.
        /// </summary>
        NotEqual,

        /// <summary>
        /// Represents !.StartsWith().
        /// </summary>
        NotStartsWith,

        /// <summary>
        /// Represents .StartsWith().
        /// </summary>
        StartsWith,
    }
}