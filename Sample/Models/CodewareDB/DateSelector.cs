using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("DateSelectors", Schema = "dbo")]
  public class DateSelector
  {
    [Key]
    public string Code
    {
      get;
      set;
    }
    public string Description
    {
      get;
      set;
    }
    public int Sort
    {
      get;
      set;
    }
  }
}
