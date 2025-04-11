using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


public class Product
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Ma { get; set; }
    public string Ten { get; set; }
    public double DonGia { get; set; }
}
