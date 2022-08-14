using Business.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DataAccess.Database;

namespace DataAccess.Services;

public class TrafficLightsService
{
    private readonly IMongoCollection<TrafficLight> _TrafficLights;

    public TrafficLightsService(
        IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.Database);

        _TrafficLights = mongoDatabase.GetCollection<TrafficLight>("TrafficLights");
    }

    public async Task<List<TrafficLight>> GetAsync() =>
        await _TrafficLights.Find(_ => true).ToListAsync();

    public async Task<TrafficLight?> GetAsync(String id) =>
        await _TrafficLights.Find(x => x.ID == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TrafficLight newTrafficLight) =>
        await _TrafficLights.InsertOneAsync(newTrafficLight);

    public async Task UpdateAsync(string id, TrafficLight updatedTrafficLight) =>
        await _TrafficLights.ReplaceOneAsync(x => x.ID == id, updatedTrafficLight);

    public async Task RemoveAsync(string id) =>
        await _TrafficLights.DeleteOneAsync(x => x.ID == id);
}