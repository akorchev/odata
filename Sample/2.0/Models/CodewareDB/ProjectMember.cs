using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("ProjectMembers", Schema = "dbo")]
  public class ProjectMember
  {
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    [Key]
    public int ProjectID
    {
      get;
      set;
    }

    [ForeignKey("ProjectID")]
    public Project Project { get; set; }
    [Key]
    public int UserID
    {
      get;
      set;
    }

    [ForeignKey("UserID")]
    public User User { get; set; }
  }
}
