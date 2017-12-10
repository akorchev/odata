using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Users", Schema = "dbo")]
  public class User
  {
    public bool Active
    {
      get;
      set;
    }
    public bool Administrator
    {
      get;
      set;
    }
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public string CurrentCompanyCode
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
    public string DisplayName
    {
      get;
      set;
    }
    public bool Enabled
    {
      get;
      set;
    }
    public string ExtranetName
    {
      get;
      set;
    }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    public string Password
    {
      get;
      set;
    }
    public int RoleID
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID
    {
      get;
      set;
    }


    [InverseProperty("User")]
    public ICollection<Detail> Details { get; set; }

    [InverseProperty("User")]
    public ICollection<KnowledgeBase> KnowledgeBases { get; set; }

    [InverseProperty("User")]
    public ICollection<CustomerKnowledgeBase> CustomerKnowledgeBases { get; set; }

    [InverseProperty("User")]
    public ICollection<Project> Projects { get; set; }

    [InverseProperty("User")]
    public ICollection<ProjectMember> ProjectMembers { get; set; }

    [InverseProperty("User")]
    public ICollection<SupportIssue> SupportIssues { get; set; }
    public string UserName
    {
      get;
      set;
    }
    public string eMail
    {
      get;
      set;
    }
    public string sqlUserName
    {
      get;
      set;
    }
  }
}
