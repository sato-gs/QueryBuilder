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
    public bool IsStocked { get; set;}
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
                Price = i * 100d,
                IsStocked = i % 2 != 0,
            };
            products.Add(product);
        }
        return products.AsQueryable();
    }
}
```
#### Example 1
> Build an expression for (p => p.Product == 1)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .Equal(nameof(Product.ProductId), 1)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().Where(query);
```
#### Example 2
> Build an expression for (p => p.Price > 2500)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .GreaterThan(nameof(Product.Price), 2500d)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().Where(query);
```
#### Example 3
> Build an expression for (p => [10, 20, 30, 40, 50].Contains(p.ProductId))
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .Contains(new int[] { 10, 20, 30, 40, 50 }, nameof(Product.ProductId))
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().Where(query);
```
#### Example 4
> Build an expression for (p => p.Name.StartsWith("Product 1") && p.Price > 1500)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .StartsWith(nameof(Product.Name), "Product 1")
                          .AndAlso()
                          .GreaterThan(nameof(Product.Price), 1500d)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().Where(query);
```
#### Example 5
> Build an expression for (p => p.Name.EndsWith("Product 50") || p.Price < 1500)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .EndsWith(nameof(Product.Name), "Product 50")
                          .OrElse()
                          .LessThan(nameof(Product.Price), 1500d)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().Where(query);
```
#### Example 6
> Build an expression for (p => p.ProductId < 25 && p.IsStocked || p.Price > 2500)
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .LessThan(nameof(Product.ProductId), 25)
                          .AndAlso()
                          .Equal(nameof(Product.IsStocked), true)
                          .OrElse()
                          .GreaterThan(nameof(Product.Price), 2500d)
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().Where(query);
```
#### Example 7
> Build an expression for (p => p.ProductId < 25 && (p.IsStocked || p.Price > 2500))
```csharp
var query = new QueryBuilder<Product>()
                      .Start()
                          .LessThan(nameof(Product.ProductId), 25)
                          .AndAlso()
                          .Start()
                              .Equal(nameof(Product.IsStocked), true)
                              .OrElse()
                              .GreaterThan(nameof(Product.Price), 2500d)
                          .End()
                      .End()
                      .Build();
var products = ProductFactory.GetProducts().Where(query);
```