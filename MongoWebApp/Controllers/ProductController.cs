using MongoDB.Driver;
using MongoWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class ProductController : Controller
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductController()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("Product");
        _productCollection = database.GetCollection<Product>("Products");
    }

    public IActionResult Index()
    {
        var products = _productCollection.Find(_ => true).ToList();
        return View(products);
    }

    // 👉 Thêm chức năng lọc
    [HttpPost]
    public IActionResult Filter(string ten, double? minPrice, double? maxPrice)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(ten))
        {
            filter &= builder.Regex(p => p.Ten, new BsonRegularExpression(ten, "i"));
        }
        if (minPrice.HasValue)
        {
            filter &= builder.Gte(p => p.DonGia, minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            filter &= builder.Lte(p => p.DonGia, maxPrice.Value);
        }

        var result = _collection.Find(filter).ToList();
        return View("Index", result);
    }
}
