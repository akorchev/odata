using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("KnowledgeBaseScriptInit", Schema = "dbo")]
  public class KnowledgeBaseScriptInit
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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int KBScriptInitID
    {
      get;
      set;
    }


    [InverseProperty("KnowledgeBaseScriptInit")]
    public ICollection<KnowledgeBase> KnowledgeBases { get; set; }
    public string KBScriptType
    {
      get;
      set;
    }

    [ForeignKey("KBScriptType")]
    public KnowledgeBaseScriptType KnowledgeBaseScriptType { get; set; }
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
