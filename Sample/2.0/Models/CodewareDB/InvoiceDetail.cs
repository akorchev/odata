using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("InvoiceDetails", Schema = "dbo")]
  public class InvoiceDetail
  {
    public DateTime CreationDate
    {
      get;
      set;
    }
    public string Description
    {
      get;
      set;
    }
    public decimal Discount
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InvoiceDetailID
    {
      get;
      set;
    }


    [InverseProperty("InvoiceDetail")]
    public ICollection<Detail> Details { get; set; }
    public int InvoiceID
    {
      get;
      set;
    }
    public decimal? Quantity
    {
      get;
      set;
    }
    public bool ShowDetails
    {
      get;
      set;
    }
    public decimal? UnitPrice
    {
      get;
      set;
    }
    public string VatCode
    {
      get;
      set;
    }

    [ForeignKey("VatCode")]
    public VatCode VatCode1 { get; set; }
  }
}
