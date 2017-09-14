using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Data.Models.Common;

namespace Server.Data.Common
{
  public interface IMongoDbRepository<T>
    where T : BaseModel
  {
    Task<IEnumerable<T>> GetAll();

    Task<T> GetById(string id);

    Task<T> Create(T entity);

    Task<bool> Update(T entity);

    Task<bool> Delete(string id);
  }
}
