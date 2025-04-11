using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoWebApp.Models;

public class ProductController : Controller
{
    private readonly IMongoCollection<Product> _collection;

    public ProductController()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("QuanLySanPham");
        _collection = database.GetCollection<Product>("Product");
    }

    public IActionResult Index()
    {
        var products = _collection.Find(Builders<Product>.Filter.Empty).ToList();
        return View(products);
    }

    [HttpPost]
    public IActionResult Filter(string ten, double? minPrice, double? maxPrice)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(ten))
        {
            filter &= builder.Regex("Ten", new BsonRegularExpression(ten, "i")); // tìm kiếm không phân biệt hoa thường
        }

        if (minPrice.HasValue)
        {
            filter &= builder.Gte("DonGia", minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            filter &= builder.Lte("DonGia", maxPrice.Value);
        }

        var filteredProducts = _collection.Find(filter).ToList();
        return View("Index", filteredProducts);
    }
}
