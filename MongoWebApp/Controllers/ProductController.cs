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
            filter &= builder.Regex("Ten", new BsonRegularExpression(ten, "i")); // không phân biệt hoa thường
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

    public IActionResult SortByDonGiaAsc()
    {
        var products = _collection.Find(FilterDefinition<Product>.Empty)
                                  .SortBy(p => p.DonGia)
                                  .ToList();
        return View("Index", products);
    }

    public IActionResult SortByDonGiaDesc()
    {
        var products = _collection.Find(FilterDefinition<Product>.Empty)
                                  .SortByDescending(p => p.DonGia)
                                  .ToList();
        return View("Index", products);
    }

    public IActionResult FilterByDonGia(double min, double max)
    {
        var filter = Builders<Product>.Filter.Gte(p => p.DonGia, min) &
                     Builders<Product>.Filter.Lte(p => p.DonGia, max);

        var products = _collection.Find(filter).ToList();
        return View("Index", products);
    }

    public IActionResult FilterAndSort(double min, double max)
    {
        var filter = Builders<Product>.Filter.Gte(p => p.DonGia, min) &
                     Builders<Product>.Filter.Lte(p => p.DonGia, max);

        var products = _collection.Find(filter)
                                  .SortBy(p => p.DonGia)
                                  .ToList();
        return View("Index", products);
    }

    public IActionResult SortDonGiaAscThenMaDesc()
    {
        var sort = Builders<Product>.Sort.Ascending(p => p.DonGia)
                                         .Descending(p => p.Ma);

        var products = _collection.Find(FilterDefinition<Product>.Empty)
                                  .Sort(sort)
                                  .ToList();
        return View("Index", products);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        _collection.InsertOne(product);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(string id)
    {
        var objectId = ObjectId.Parse(id);
        var product = _collection.Find(p => p.Id == objectId).FirstOrDefault();
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(string id, Product updated)
    {
        var objectId = ObjectId.Parse(id);
        updated.Id = objectId;
        _collection.ReplaceOne(p => p.Id == objectId, updated);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(string id)
    {
        var objectId = ObjectId.Parse(id);
        var product = _collection.Find(p => p.Id == objectId).FirstOrDefault();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(string id)
    {
        var objectId = ObjectId.Parse(id);
        _collection.DeleteOne(p => p.Id == objectId);
        return RedirectToAction("Index");
    }

    public IActionResult Details(string id)
    {
        var objectId = ObjectId.Parse(id);
        var product = _collection.Find(p => p.Id == objectId).FirstOrDefault();
        return View(product);
    }
}
