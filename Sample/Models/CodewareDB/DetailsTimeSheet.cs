using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("DetailsTimeSheet", Schema = "dbo")]
  public class DetailsTimeSheet
  {
    public int? CategorieID
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DetailHelperID
    {
      get;
      set;
    }
    public DateTime EntryDate
    {
      get;
      set;
    }
    public int Kilometers
    {
      get;
      set;
    }
    public int? ProjectID
    {
      get;
      set;
    }
    public TimeSpan? TravelEnd1
    {
      get;
      set;
    }
    public TimeSpan? TravelEnd2
    {
      get;
      set;
    }
    public TimeSpan? TravelStart1
    {
      get;
      set;
    }
    public TimeSpan? TravelStart2
    {
      get;
      set;
    }
    public TimeSpan? WorkEnd
    {
      get;
      set;
    }
    public TimeSpan? WorkEnd1
    {
      get;
      set;
    }
    public TimeSpan? WorkEnd2
    {
      get;
      set;
    }
    public TimeSpan? WorkStart1
    {
      get;
      set;
    }
    public TimeSpan? WorkStart2
    {
      get;
      set;
    }
    public TimeSpan? WorkStart3
    {
      get;
      set;
    }
  }
}
