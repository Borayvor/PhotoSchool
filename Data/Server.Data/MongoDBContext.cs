using System;
using System.Security.Authentication;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Server.Data
{
  public class MongoDbContext : IMongoDbContext
  {
    private IMongoDatabase Database { get; }

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
      try
      {
        MongoClientSettings mongoClientSettings = MongoClientSettings
          .FromUrl(new MongoUrl(settings.Value.ConnectionString));

        if (settings.Value.IsSSL)
        {
          mongoClientSettings.SslSettings = new SslSettings
          {
            EnabledSslProtocols = SslProtocols.Tls12
          };
        }

        var mongoClient = new MongoClient(mongoClientSettings);

        if (mongoClient != null)
        {
          this.Database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Can not access to db server.", ex);
      }
    }

    public IMongoCollection<T> Set<T>()
      where T : class
    {
      string collectionName = typeof(T).Name + "s";

      var collection = this.Database.GetCollection<T>(collectionName);

      return collection;
    }
  }
}
