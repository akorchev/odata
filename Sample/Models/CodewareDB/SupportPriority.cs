using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("SupportPriority", Schema = "dbo")]
  public class SupportPriority
  {
    public string Description
    {
      get;
      set;
    }
    [Key]
    public string Priority
    {
      get;
      set;
    }


    [InverseProperty("SupportPriority")]
    public ICollection<SupportIssue> SupportIssues { get; set; }
  }
}
