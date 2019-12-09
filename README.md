# QueryBuilder

A small library that enables users to dynamically build lambda expressions via expression trees, which can be utilized to filter collections of objects based on user input, create generic repositories for EF/EF Core, etc

## How Tos

- Expressions built through`QueryBuilder<T>()`can be passed to any functions expecting`Expression<Func<TSource, bool>>`predicates such as`First()`,`FirstOrDefault()`,`Last()`,`LastOrDefault()`,`Where()`,`Any()`,`All()`, etc.
- Expressions can be built by starting with`Start()`, followed by conditions (e.g. `Equal()`,`GreaterThan()`, `GreaterThanOrEqual()`,`LessThan()`,`LessThanOrEqual()`), and ending with`End()`.
  - Conditions can be chained via either`AndAlso()`or`OrElse()`.
  - Conditions can be nested by nesting another sequence of `Start()`+ conditions + `End()`.

## Usage Example

#### Example Classes
```csharp
public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}

public static class ProductFactory {
    public static IQueryable<Product> GetProducts()
    {
        var products = new List<Product>();
        for (var i = 1; i <= 50; i++)
        {
            var product = new Product()
            {
                ProductId = i,
                Name = $"Product {i}",
                Price = i * 10.0d,
            };
            products.Add(product);
        }
        return products.AsQueryable();
    }
}
```
#### Example 1
> Build an expression for Product == 1
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .Equal(nameof(Product.ProductId), 1)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().where(query);
```
#### Example 2
> Build an expression for Price > 250
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .GreaterThan(nameof(Product.Price), 250.0d)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().where(query);
```
#### Example 3
> Build an expression for ([10, 20, 30, 40, 50]).Contains(ProductId)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .Contains(new int[] { 10, 20, 30, 40, 50 }, nameof(Product.ProductId))
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().where(query);
```
#### Example 4
> Build an expression for (Name).StartsWith("Product 1") && Price > 150
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .StartsWith(nameof(Product.Name), "Product 1")
                          .AndAlso()
                          .GreaterThan(nameof(Product.Price), 150.0d)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().where(query);
```
#### Example 5
> Build an expression for (Name).EndsWith("Product 50") || Price < 150
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .EndsWith(nameof(Product.Name), "Product 50")
                          .OrElse()
                          .LessThan(nameof(Product.Price), 150.0d)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().where(query);
```
#### Example 6
> Build an expression for (Name).StartsWith("Product 1") && (([10, 20, 30, 40, 50]).Contains(ProductId) && Price <= 150)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .StartsWith(nameof(Product.Name), "Product 1")
                          .AndAlso()
                          .Start()
                              .Contains(new int[] { 10, 20, 30, 40, 50 }, nameof(Product.ProductId))
                              .AndAlso()
                              .LessThanOrEqual(nameof(Product.Price), 150.0d)
                          .End()
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().where(query);
```
#### Example 7
> Build an expression for (Name).StartsWith("Product 1") || (([10, 20, 30, 40, 50]).Contains(ProductId) || Price <= 150)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .StartsWith(nameof(Product.Name), "Product 1")
                          .OrElse()
                          .Start()
                              .Contains(new int[] { 10, 20, 30, 40, 50 }, nameof(Product.ProductId))
                              .OrElse()
                              .LessThanOrEqual(nameof(Product.Price), 150.0d)
                          .End()
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().where(query);
```















```
