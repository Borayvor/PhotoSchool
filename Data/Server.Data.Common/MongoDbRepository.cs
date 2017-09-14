using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Server.Data.Models.Common;

namespace Server.Data.Common
{
  public class MongoDbRepository<T> : IMongoDbRepository<T>
    where T : BaseModel
  {
    public MongoDbRepository(IMongoDbContext context)
    {
      this.Context = context;
      this.DbSet = this.Context.Set<T>();
    }

    private IMongoDbContext Context { get; set; }

    private IMongoCollection<T> DbSet { get; set; }

    public async Task<IEnumerable<T>> GetAll()
    {
      try
      {
        return await this.DbSet.Find(_ => true).ToListAsync();
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<T> GetById(string id)
    {
      try
      {
        var filter = Builders<T>.Filter.Eq("Id", id);

        return await this.DbSet.Find(filter).FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<T> Create(T entity)
    {
      try
      {
        await this.DbSet.InsertOneAsync(entity);

        return entity;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<bool> Update(T entity)
    {
      try
      {
        var result = await this.DbSet
          .ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, new UpdateOptions { IsUpsert = true });

        return result.IsAcknowledged;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }

    public async Task<bool> Delete(string id)
    {
      try
      {
        var filter = Builders<T>.Filter.Eq("Id", id);
        var update = Builders<T>.Update
                            .Set(e => e.ModifiedOn, DateTime.UtcNow)
                            .Set(e => e.IsDeleted, true);

        var result = await this.DbSet.UpdateOneAsync(filter, update);

        return result.IsAcknowledged;
      }
      catch (Exception ex)
      {
        // log or manage the exception
        throw ex;
      }
    }
  }
}
