using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models.Northwind
{
  [Table("Shippers", Schema = "dbo")]
  public class Shipper
  {
    public string CompanyName
    {
      get;
      set;
    }
    public string Phone
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShipperID
    {
      get;
      set;
    }


    [InverseProperty("Shipper")]
    public ICollection<Order> Orders { get; set; }
  }
}
