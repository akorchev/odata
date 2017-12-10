using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Unit", Schema = "dbo")]
  public class Unit
  {
    [Key]
    public string PK_Days
    {
      get;
      set;
    }
  }
}
