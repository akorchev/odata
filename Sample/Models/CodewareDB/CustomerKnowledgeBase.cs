using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("CustomerKnowledgeBase", Schema = "dbo")]
  public class CustomerKnowledgeBase
  {
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
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerKBID
    {
      get;
      set;
    }
    public int KBID
    {
      get;
      set;
    }

    [ForeignKey("KBID")]
    public KnowledgeBase KnowledgeBase { get; set; }
  }
}
