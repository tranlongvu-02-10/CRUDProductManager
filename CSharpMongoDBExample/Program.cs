using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Kết nối tới MongoDB Server
        MongoClient client = new MongoClient("mongodb://localhost:27017");

        // Kết nối tới database QuanLySanPham
        IMongoDatabase database = client.GetDatabase("QuanLySanPham");

        // Kết nối tới collection "Product"
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Product");

        // Tạo điều kiện truy vấn: DonGia = 20
        var builder = Builders<BsonDocument>.Filter;
        var query = builder.Eq("DonGia", 20);

        // Truy vấn dữ liệu với điều kiện
        List<BsonDocument> documents = await collection.Find(query).ToListAsync();

        // Duyệt và hiển thị kết quả
        foreach (BsonDocument document in documents)
        {
            string ma = document["Ma"].AsString;
            string ten = document["Ten"].AsString;
            double gia = document["DonGia"].AsDouble;

            Console.WriteLine($"Mã: {ma}, Tên: {ten}, Đơn giá: {gia}");
        }
    }
}
