using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService
{
    private readonly IMongoCollection<Product> _products;

    public ProductService()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("QuanLySanPham");
        _products = database.GetCollection<Product>("Product");
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _products.Find(_ => true).ToListAsync();
    }
}
