using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("KnowledgeBase", Schema = "dbo")]
  public class KnowledgeBase
  {
    public string Content
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
    public string Description
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int KBID
    {
      get;
      set;
    }


    [InverseProperty("KnowledgeBase")]
    public ICollection<Detail> Details { get; set; }

    [InverseProperty("KnowledgeBase")]
    public ICollection<CustomerKnowledgeBase> CustomerKnowledgeBases { get; set; }
    public int? KBScriptInitID
    {
      get;
      set;
    }

    [ForeignKey("KBScriptInitID")]
    public KnowledgeBaseScriptInit KnowledgeBaseScriptInit { get; set; }
    public string KBScriptType
    {
      get;
      set;
    }

    [ForeignKey("KBScriptType")]
    public KnowledgeBaseScriptType KnowledgeBaseScriptType { get; set; }
    public int? KBSubTypeID
    {
      get;
      set;
    }

    [ForeignKey("KBSubTypeID")]
    public KnowledegeBaseSubType KnowledegeBaseSubType { get; set; }
    public int KBTypeID
    {
      get;
      set;
    }

    [ForeignKey("KBTypeID")]
    public KnowledgeBaseType KnowledgeBaseType { get; set; }
    public string Script
    {
      get;
      set;
    }
    public string Solution
    {
      get;
      set;
    }
    public string URL
    {
      get;
      set;
    }
  }
}
