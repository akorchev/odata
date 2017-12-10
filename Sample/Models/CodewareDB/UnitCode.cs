using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("UnitCodes", Schema = "dbo")]
  public class UnitCode
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
    public bool ShowTime
    {
      get;
      set;
    }
    [Key]
    [Column("UnitCode")]
    public string UnitCode1
    {
      get;
      set;
    }


    [InverseProperty("UnitCode1")]
    public ICollection<Detail> Details { get; set; }

    [InverseProperty("UnitCode1")]
    public ICollection<Project> Projects { get; set; }

    [InverseProperty("UnitCode1")]
    public ICollection<Category> Categories { get; set; }
    public string UnitDescription
    {
      get;
      set;
    }
  }
}
