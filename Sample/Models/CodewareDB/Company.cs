using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Companies", Schema = "dbo")]
  public class Company
  {
    public string Adres
    {
      get;
      set;
    }
    public string BancAccount
    {
      get;
      set;
    }
    public string City
    {
      get;
      set;
    }
    [Column("Company")]
    public string Company1
    {
      get;
      set;
    }
    [Key]
    public string CompanyCode
    {
      get;
      set;
    }


    [InverseProperty("Company")]
    public ICollection<Detail> Details { get; set; }

    [InverseProperty("Company")]
    public ICollection<Invoice> Invoices { get; set; }
    public string CountryCode
    {
      get;
      set;
    }
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public string Email
    {
      get;
      set;
    }
    public string Fax
    {
      get;
      set;
    }
    public string HR
    {
      get;
      set;
    }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    public string Phone
    {
      get;
      set;
    }
    public string SubTitle
    {
      get;
      set;
    }
    public string VATNumber
    {
      get;
      set;
    }
    public string ZipCode
    {
      get;
      set;
    }
  }
}
