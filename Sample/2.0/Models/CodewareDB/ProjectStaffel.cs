using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("ProjectStaffels", Schema = "dbo")]
  public class ProjectStaffel
  {
    public string Description
    {
      get;
      set;
    }
    public int ProjectID
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectStaffelID
    {
      get;
      set;
    }
    public string UnitCode
    {
      get;
      set;
    }
    public decimal UnitValue
    {
      get;
      set;
    }
  }
}
