using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Categories", Schema = "dbo")]
  public class Category
  {
    public string Abbreviation
    {
      get;
      set;
    }
    public bool Active
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryID
    {
      get;
      set;
    }


    [InverseProperty("Category")]
    public ICollection<Detail> Details { get; set; }

    [InverseProperty("Category")]
    public ICollection<Project> Projects { get; set; }
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public decimal Discount
    {
      get;
      set;
    }
    public string EstDurUnit
    {
      get;
      set;
    }

    [ForeignKey("EstDurUnit")]
    public DatePeriod DatePeriod { get; set; }
    public decimal? EstDuration
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
    public int ProjectID
    {
      get;
      set;
    }

    [ForeignKey("ProjectID")]
    public Project Project { get; set; }
    public string UnitCode
    {
      get;
      set;
    }

    [ForeignKey("UnitCode")]
    public UnitCode UnitCode1 { get; set; }
    public decimal UnitPrice
    {
      get;
      set;
    }
  }
}
