using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("RolePermissions", Schema = "dbo")]
  public class RolePermission
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
