using Business.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DataAccess.Database;

namespace DataAccess.Services;

public class RoadsService
{
    private readonly IMongoCollection<Road> _Roads;

    public RoadsService()
    {
        var mongoClient = new MongoClient("mongodb+srv://admin:admin@smartlights.0tdmts4.mongodb.net/?retryWrites=true&w=majority");
        var mongoDatabase = mongoClient.GetDatabase("SmartLights");

        _Roads = mongoDatabase.GetCollection<Road>("Roads");
    }

    public async Task<List<Road>> GetAsync() =>
        await _Roads.Find(_ => true).ToListAsync();

    public async Task<Road?> GetAsync(String id) =>
        await _Roads.Find(x => x.ID == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Road newRoad) =>
        await _Roads.InsertOneAsync(newRoad);

    public async Task UpdateAsync(string id, Road updatedRoad) =>
        await _Roads.ReplaceOneAsync(x => x.ID == id, updatedRoad);

    public async Task RemoveAsync(string id) =>
        await _Roads.DeleteOneAsync(x => x.ID == id);
}