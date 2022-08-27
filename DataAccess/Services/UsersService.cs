using Business.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DataAccess.Database;

namespace DataAccess.Services;

public class UsersService
{
    private readonly IMongoCollection<User> _Users;

    public UsersService()
    {
        var mongoClient = new MongoClient("mongodb+srv://admin:admin@smartlights.0tdmts4.mongodb.net/?retryWrites=true&w=majority");
        var mongoDatabase = mongoClient.GetDatabase("SmartLights");

        _Users = mongoDatabase.GetCollection<User>("Users");
    }

    public async Task<List<User>> GetAsync() =>
        await _Users.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(String id) =>
        await _Users.Find(x => x.ID == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _Users.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _Users.ReplaceOneAsync(x => x.ID == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _Users.DeleteOneAsync(x => x.ID == id);
}