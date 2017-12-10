using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("DetailTypes", Schema = "dbo")]
  public class DetailType
  {
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public string Description
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
    public string Type
    {
      get;
      set;
    }


    [InverseProperty("DetailType")]
    public ICollection<Detail> Details { get; set; }
  }
}
