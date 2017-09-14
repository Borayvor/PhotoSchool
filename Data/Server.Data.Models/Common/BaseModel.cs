using System;

namespace Server.Data.Models.Common
{
  public abstract class BaseModel : IBaseModel, IAuditInfo, IDeletableEntity
  {
    public string Id { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
  }
}
