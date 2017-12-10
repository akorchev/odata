using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("DropdownLists", Schema = "dbo")]
  public class DropdownList
  {
    [Key]
    public string Code
    {
      get;
      set;
    }
    public string Omschrijving
    {
      get;
      set;
    }
    public string Type
    {
      get;
      set;
    }
  }
}
