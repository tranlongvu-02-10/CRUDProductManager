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

        // Truy vấn toàn bộ dữ liệu
        List<BsonDocument> documents = await collection.Find(new BsonDocument()).ToListAsync();


        foreach (BsonDocument document in documents)
        {
        //Cột Ma (là thuộc tính Ma của đối tượng Product đang duyệt) có kiểu chuỗi
        string ma = document["Ma"].AsString;
        //Cột Ten (là thuộc tính Ten của đối tượng Product đang duyệt) có kiểu chuỗi
        string ten = document["Ten"].AsString;
        //Cột DonGia (là thuộc tính DonGia của đối tượng Product đang duyệt) có kiểu Double
        double gia = document["DonGia"].AsDouble;
        Console.WriteLine($"Mã: {ma}, Tên: {ten}, Đơn giá: {gia}");
        }
    }
}
