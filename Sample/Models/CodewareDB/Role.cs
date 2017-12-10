using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Roles", Schema = "dbo")]
  public class Role
  {
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    public string Name
    {
      get;
      set;
    }
    [Key]
    public int RoleID
    {
      get;
      set;
    }
  }
}
