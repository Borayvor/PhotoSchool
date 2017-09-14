using System;

namespace Server.Data.Models.Common
{
  public interface IDeletableEntity
  {
    bool IsDeleted { get; set; }

    DateTime? DeletedOn { get; set; }
  }
}
