using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("RolePermissionSet", Schema = "dbo")]
  public class RolePermissionSet
  {
    [Key]
    public string PermissionId
    {
      get;
      set;
    }
    [Key]
    public string RoleName
    {
      get;
      set;
    }
  }
}
