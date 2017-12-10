using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models.Northwind
{
  [Table("Territories", Schema = "dbo")]
  public class Territory
  {
    public int RegionID
    {
      get;
      set;
    }

    [ForeignKey("RegionID")]
    public Region Region { get; set; }
    public string TerritoryDescription
    {
      get;
      set;
    }
    [Key]
    public string TerritoryID
    {
      get;
      set;
    }


    [InverseProperty("Territory")]
    public ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }
  }
}
