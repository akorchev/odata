using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("VatCodes", Schema = "dbo")]
  public class VatCode
  {
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
    [Key]
    [Column("VatCode")]
    public string VatCode1
    {
      get;
      set;
    }


    [InverseProperty("VatCode1")]
    public ICollection<Detail> Details { get; set; }

    [InverseProperty("VatCode")]
    public ICollection<Customer> Customers { get; set; }

    [InverseProperty("VatCode1")]
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    public string VatDescription
    {
      get;
      set;
    }
    public decimal VatMultiplier
    {
      get;
      set;
    }
  }
}
