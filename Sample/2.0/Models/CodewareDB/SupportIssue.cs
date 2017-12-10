using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("SupportIssues", Schema = "dbo")]
  public class SupportIssue
  {
    public int? AssignedTo
    {
      get;
      set;
    }

    //[ForeignKey("AssignedTo")]
    //public User User1 { get; set; }
    public DateTime? ClosedOn
    {
      get;
      set;
    }
    public int CreatedBy
    {
      get;
      set;
    }

    [ForeignKey("CreatedBy")]
    public User User { get; set; }
    public DateTime CreatedOn
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
    public string Priority
    {
      get;
      set;
    }

    [ForeignKey("Priority")]
    public SupportPriority SupportPriority { get; set; }
    public string Remarkt
    {
      get;
      set;
    }
    public string State
    {
      get;
      set;
    }

    [ForeignKey("State")]
    public SupportState SupportState { get; set; }
    public int SupportCategoryID
    {
      get;
      set;
    }

    [ForeignKey("SupportCategoryID")]
    public SupportCategory SupportCategory { get; set; }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SupportID
    {
      get;
      set;
    }
  }
}
