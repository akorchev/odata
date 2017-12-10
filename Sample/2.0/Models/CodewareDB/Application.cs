using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Applications", Schema = "dbo")]
  public class Application
  {
    [Key]
    public Guid ApplicationId
    {
      get;
      set;
    }
    public string ApplicationName
    {
      get;
      set;
    }
    public string Description
    {
      get;
      set;
    }
  }
}
