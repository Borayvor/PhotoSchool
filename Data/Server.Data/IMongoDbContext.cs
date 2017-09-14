using MongoDB.Driver;

namespace Server.Data
{
  public interface IMongoDbContext
  {
    IMongoCollection<T> Set<T>()
      where T : class;
  }
}