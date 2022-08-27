using Business.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DataAccess.Database;

namespace DataAccess.Services;

public class TrafficLightsService
{
    private readonly IMongoCollection<TrafficLight> _TrafficLights;

    public TrafficLightsService()
    {
        var mongoClient = new MongoClient("mongodb+srv://admin:admin@smartlights.0tdmts4.mongodb.net/?retryWrites=true&w=majority");
        var mongoDatabase = mongoClient.GetDatabase("SmartLights");

        _TrafficLights = mongoDatabase.GetCollection<TrafficLight>("TrafficLights");
    }

    public async Task<List<TrafficLight>> GetAsync() =>
        await _TrafficLights.Find(_ => true).ToListAsync();

    public async Task<TrafficLight?> GetAsync(String id) =>
        await _TrafficLights.Find(x => x.ID == id).FirstOrDefaultAsync();
    public async Task<List<TrafficLight>> GetByRoadIDAsync(String roadID) =>
        await _TrafficLights.Find(x => x.RoadAID == roadID || x.RoadBID == roadID).ToListAsync();
    public async Task CreateAsync(TrafficLight newTrafficLight) =>
        await _TrafficLights.InsertOneAsync(newTrafficLight);

    public async Task UpdateAsync(string id, TrafficLight updatedTrafficLight) =>
        await _TrafficLights.ReplaceOneAsync(x => x.ID == id, updatedTrafficLight);

    public async Task RemoveAsync(string id) =>
        await _TrafficLights.DeleteOneAsync(x => x.ID == id);
}