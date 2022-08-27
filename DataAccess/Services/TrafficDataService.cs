using Business.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DataAccess.Database;

namespace DataAccess.Services;

public class TrafficDataService
{
    private readonly IMongoCollection<TrafficData> _TrafficData;

    public TrafficDataService()
    {
        var mongoClient = new MongoClient("mongodb+srv://admin:admin@smartlights.0tdmts4.mongodb.net/?retryWrites=true&w=majority");
        var mongoDatabase = mongoClient.GetDatabase("SmartLights");

        _TrafficData = mongoDatabase.GetCollection<TrafficData>("TrafficData");
    }

    public async Task<List<TrafficData>> GetAsync() =>
        await _TrafficData.Find(_ => true).ToListAsync();

    public async Task<List<TrafficData>> GetLastSecondsAsync(int seconds) =>
        await _TrafficData.Find(x => x.Generated > DateTime.Now.AddSeconds(-seconds)).ToListAsync();

    public async Task<TrafficData?> GetAsync(String id) =>
        await _TrafficData.Find(x => x.ID == id).FirstOrDefaultAsync();

    public async Task<List<TrafficData>> GetByTrafficLightIDAsync(String trafficLightID) =>
        await _TrafficData.Find(x => x.TrafficLightID == trafficLightID).ToListAsync();

    public async Task CreateAsync(TrafficData newTrafficData) =>
        await _TrafficData.InsertOneAsync(newTrafficData);

    public async Task UpdateAsync(string id, TrafficData updatedTrafficData) =>
        await _TrafficData.ReplaceOneAsync(x => x.ID == id, updatedTrafficData);

    public async Task RemoveAsync(string id) =>
        await _TrafficData.DeleteOneAsync(x => x.ID == id);
}