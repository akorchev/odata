using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("DatePeriods", Schema = "dbo")]
  public class DatePeriod
  {
    [Key]
    public string Code
    {
      get;
      set;
    }


    [InverseProperty("DatePeriod")]
    public ICollection<Category> Categories { get; set; }
    public string Omschrijving
    {
      get;
      set;
    }
    public int Sortering
    {
      get;
      set;
    }
  }
}
