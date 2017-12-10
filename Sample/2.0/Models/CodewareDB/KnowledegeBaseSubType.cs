using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("KnowledegeBaseSubTypes", Schema = "dbo")]
  public class KnowledegeBaseSubType
  {
    public string Description
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int KBSubTypeID
    {
      get;
      set;
    }


    [InverseProperty("KnowledegeBaseSubType")]
    public ICollection<KnowledgeBase> KnowledgeBases { get; set; }
    public int KBTypeID
    {
      get;
      set;
    }

    [ForeignKey("KBTypeID")]
    public KnowledgeBaseType KnowledgeBaseType { get; set; }
  }
}
