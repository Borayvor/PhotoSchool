namespace Server.Data.Models.Common
{
  public interface IBaseModel : IAuditInfo, IDeletableEntity
  {
    string Id { get; set; }
  }
}