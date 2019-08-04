using System;
using System.Collections.Generic;
using System.Linq;
using QueryBuilder.QueryBuilders;
using QueryBuilderTests.Helpers;
using Xunit;

namespace QueryBuilderTests.Queries
{
    public class QueryTests
    {
        public IQueryable<Product> GetProducts()
        {
            var products = new List<Product>();
            for (var i = 1; i <= 50; i++)
            {
                var product = new Product()
                {
                    ProductId = i,
                    Name = $"Product {i}",
                    Price = Convert.ToDouble(i)
                };
                products.Add(product);
            }

            return products.AsQueryable();
        }

        [Fact]
        public void PlayGround()
        {
            var products = GetProducts();

            // case1
            var query1 = new QueryBuilder<Product>()
                                .Start()
                                    .Equal(nameof(Product.ProductId), 1)
                                .End()
                                .Build();
            var products1 = products.Where(query1);

            // case2
            var query2 = new QueryBuilder<Product>()
                                .Start()
                                    .Equal(nameof(Product.Name), "Product 1")
                                .End()
                                .Build();
            var products2 = products.Where(query2);

            // case3
            var query3 = new QueryBuilder<Product>()
                                .Start()
                                    .Contains(new int[] { 1, 5, 10 }, nameof(Product.ProductId))
                                .End()
                                .Build();
            var products3 = products.Where(query3);

            // case4
            var query4 = new QueryBuilder<Product>()
                                .Start()
                                    .StartsWith(nameof(Product.Name), "Product 1")
                                    .AndAlso()
                                    .GreaterThan(nameof(Product.Price), 10d)
                                .End()
                                .Build();

            var products4 = products.Where(query4);

            // case 5 (nesting)
            var query5 = new QueryBuilder<Product>()
                                .Start()
                                    .StartsWith(nameof(Product.Name), "Product 1")
                                    .OrElse()
                                    .Start()
                                        .Contains(new int[] { 1, 5, 10 }, nameof(Product.ProductId))
                                        .AndAlso()
                                        .LessThanOrEqual(nameof(Product.Price), 5d)
                                    .End()
                                .End()
                                .Build();

            var products5 = products.Where(query5);

        }
    }
}
