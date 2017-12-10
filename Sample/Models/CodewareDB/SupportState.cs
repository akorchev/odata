using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("SupportStates", Schema = "dbo")]
  public class SupportState
  {
    [Key]
    public string State
    {
      get;
      set;
    }


    [InverseProperty("SupportState")]
    public ICollection<SupportIssue> SupportIssues { get; set; }
    public string description
    {
      get;
      set;
    }
  }
}
