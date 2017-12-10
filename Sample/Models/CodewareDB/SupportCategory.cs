using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("SupportCategories", Schema = "dbo")]
  public class SupportCategory
  {
    public string Category
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
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SupportCategoryID
    {
      get;
      set;
    }


    [InverseProperty("SupportCategory")]
    public ICollection<SupportIssue> SupportIssues { get; set; }
  }
}
