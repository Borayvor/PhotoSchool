using Server.Data.Models.Common;

namespace Server.Data.Models
{
  public class User : BaseModel, IBaseModel, IAuditInfo, IDeletableEntity
  {
    public string Username { get; set; }

    public string Password { get; set; }
  }
}
