using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("KnowledgeBaseTypes", Schema = "dbo")]
  public class KnowledgeBaseType
  {
    public string Description
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int KBTypeID
    {
      get;
      set;
    }


    [InverseProperty("KnowledgeBaseType")]
    public ICollection<KnowledgeBase> KnowledgeBases { get; set; }

    [InverseProperty("KnowledgeBaseType")]
    public ICollection<KnowledegeBaseSubType> KnowledegeBaseSubTypes { get; set; }
  }
}
