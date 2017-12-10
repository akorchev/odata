using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Profiles", Schema = "dbo")]
  public class Profile
  {
    public DateTime LastUpdatedDate
    {
      get;
      set;
    }
    public string PropertyNames
    {
      get;
      set;
    }
    public Byte[] PropertyValueBinary
    {
      get;
      set;
    }
    public string PropertyValueStrings
    {
      get;
      set;
    }
    [Key]
    public Guid UserId
    {
      get;
      set;
    }
  }
}
