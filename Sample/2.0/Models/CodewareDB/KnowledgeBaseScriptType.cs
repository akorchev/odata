using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("KnowledgeBaseScriptTypes", Schema = "dbo")]
  public class KnowledgeBaseScriptType
  {
    public string Description
    {
      get;
      set;
    }
    public string Initialisation
    {
      get;
      set;
    }
    [Key]
    public string KBScriptType
    {
      get;
      set;
    }


    [InverseProperty("KnowledgeBaseScriptType")]
    public ICollection<KnowledgeBase> KnowledgeBases { get; set; }

    [InverseProperty("KnowledgeBaseScriptType")]
    public ICollection<KnowledgeBaseScriptInit> KnowledgeBaseScriptInits { get; set; }
    public string LoginName
    {
      get;
      set;
    }
    public string Password
    {
      get;
      set;
    }
  }
}
