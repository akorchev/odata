using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models.Northwind
{
  [Table("CustomerDemographics", Schema = "dbo")]
  public class CustomerDemographic
  {
    public string CustomerDesc
    {
      get;
      set;
    }
    [Key]
    public string CustomerTypeID
    {
      get;
      set;
    }


    [InverseProperty("CustomerDemographic")]
    public ICollection<CustomerCustomerDemo> CustomerCustomerDemos { get; set; }
  }
}
