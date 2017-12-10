using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Countries", Schema = "dbo")]
  public class Country
  {
    [Column("Country")]
    public string Country1
    {
      get;
      set;
    }
    [Key]
    public string CountryCode
    {
      get;
      set;
    }


    [InverseProperty("Country")]
    public ICollection<Customer> Customers { get; set; }
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
  }
}
