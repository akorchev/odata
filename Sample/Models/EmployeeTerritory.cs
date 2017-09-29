using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Models
{
  [Table("EmployeeTerritories", Schema = "dbo")]
  public class EmployeeTerritory
  {
    [Key]
    public int EmployeeID
    {
      get;
      set;
    }

    [ForeignKey("EmployeeID")]
    public Employee Employee { get; set; }
    [Key]
    public string TerritoryID
    {
      get;
      set;
    }

    [ForeignKey("TerritoryID")]
    public Territory Territory { get; set; }
  }
}
