using System;
using Server.Data.Models.Common;

namespace Server.Data.Models
{
  public class UserSession : BaseModel, IBaseModel, IAuditInfo, IDeletableEntity
  {
    public string OwnerUserId { get; set; }

    public string AuthToken { get; set; }

    public DateTime ExpirationDateTime { get; set; }
  }
}