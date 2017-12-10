using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Projects", Schema = "dbo")]
  public class Project
  {
    public bool Active
    {
      get;
      set;
    }
    public int? CategoryID
    {
      get;
      set;
    }

    //[ForeignKey("CategoryID")]
    //public Category Category1 { get; set; }
    public DateTime CreationDate
    {
      get;
      set;
    }
    public string CustomerCode
    {
      get;
      set;
    }

    [ForeignKey("CustomerCode")]
    public Customer Customer { get; set; }
    public string Description
    {
      get;
      set;
    }
    public DateTime? EstCompletionDate
    {
      get;
      set;
    }
    public decimal? EstDuration
    {
      get;
      set;
    }
    public int? KMCategory
    {
      get;
      set;
    }

    [ForeignKey("KMCategory")]
    public Category Category { get; set; }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    public int? ManagerUserID
    {
      get;
      set;
    }

    [ForeignKey("ManagerUserID")]
    public User User { get; set; }
    public string Name
    {
      get;
      set;
    }
    public int? ProjectCalcID
    {
      get;
      set;
    }

    [ForeignKey("ProjectCalcID")]
    public ProjectCalculation ProjectCalculation { get; set; }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectID
    {
      get;
      set;
    }


    [InverseProperty("Project")]
    public ICollection<Detail> Details { get; set; }

    [InverseProperty("Project")]
    public ICollection<Category> Categories { get; set; }

    [InverseProperty("Project")]
    public ICollection<ProjectMember> ProjectMembers { get; set; }
    public string UnitCode
    {
      get;
      set;
    }

    [ForeignKey("UnitCode")]
    public UnitCode UnitCode1 { get; set; }
    public decimal? UnitPrice
    {
      get;
      set;
    }
  }
}
