using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models.Northwind
{
  [Table("CustomerCustomerDemo", Schema = "dbo")]
  public class CustomerCustomerDemo
  {
    [Key]
    public string CustomerID
    {
      get;
      set;
    }

    [ForeignKey("CustomerID")]
    public Customer Customer { get; set; }
    [Key]
    public string CustomerTypeID
    {
      get;
      set;
    }

    [ForeignKey("CustomerTypeID")]
    public CustomerDemographic CustomerDemographic { get; set; }
  }
}
