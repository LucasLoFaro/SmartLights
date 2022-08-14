using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Business.Models
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public String? ID { get; set; }
        public String Name { get; set; }
        public UserRole Role { get; set; }
        public DateTime? LastLogin { get; set; } = DateTime.Now;
        public List<Object>? ActionsTaken { get; set; }
        
    }    

    public enum UserRole
    {
        Admin,
        User
    }
}