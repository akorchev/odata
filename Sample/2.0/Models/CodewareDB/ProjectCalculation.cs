using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("ProjectCalculations", Schema = "dbo")]
  public class ProjectCalculation
  {
    public string CalculationFormula
    {
      get;
      set;
    }
    public string Description
    {
      get;
      set;
    }
    public decimal? FixedValue
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectCalcID
    {
      get;
      set;
    }


    [InverseProperty("ProjectCalculation")]
    public ICollection<Project> Projects { get; set; }
  }
}
