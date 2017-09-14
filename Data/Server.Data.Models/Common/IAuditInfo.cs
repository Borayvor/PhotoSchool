using System;

namespace Server.Data.Models.Common
{
  public interface IAuditInfo
  {
    DateTime CreatedOn { get; set; }

    DateTime? ModifiedOn { get; set; }
  }
}
