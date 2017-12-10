using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models.Northwind
{
  [Table("Suppliers", Schema = "dbo")]
  public class Supplier
  {
    public string Address
    {
      get;
      set;
    }
    public string City
    {
      get;
      set;
    }
    public string CompanyName
    {
      get;
      set;
    }
    public string ContactName
    {
      get;
      set;
    }
    public string ContactTitle
    {
      get;
      set;
    }
    public string Country
    {
      get;
      set;
    }
    public string Fax
    {
      get;
      set;
    }
    public string HomePage
    {
      get;
      set;
    }
    public string Phone
    {
      get;
      set;
    }
    public string PostalCode
    {
      get;
      set;
    }
    public string Region
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SupplierID
    {
      get;
      set;
    }


    [InverseProperty("Supplier")]
    public ICollection<Product> Products { get; set; }
  }
}
