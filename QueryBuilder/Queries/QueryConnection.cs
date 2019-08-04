// <copyright file="QueryConnection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace QueryBuilder.Queries
{
    /// <summary>
    /// Represents a query connection that connects queries.
    /// </summary>
    public enum QueryConnection
    {
        /// <summary>
        /// Represents &&.
        /// </summary>
        AndAlso,

        /// <summary>
        /// Represents no connection.
        /// </summary>
        None,

        /// <summary>
        /// Represents ||.
        /// </summary>
        OrElse,
    }
}