using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Invoices", Schema = "dbo")]
  public class Invoice
  {
    public string CompanyCode
    {
      get;
      set;
    }

    [ForeignKey("CompanyCode")]
    public Company Company { get; set; }
    public DateTime? CreationDate
    {
      get;
      set;
    }
    public string CustomerCode
    {
      get;
      set;
    }

    [ForeignKey("CustomerCode")]
    public Customer Customer { get; set; }
    public DateTime DueDate
    {
      get;
      set;
    }
    public DateTime InvoiceDate
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InvoiceID
    {
      get;
      set;
    }


    [InverseProperty("Invoice")]
    public ICollection<Detail> Details { get; set; }
    public int InvoiceNumber
    {
      get;
      set;
    }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    public bool Locked
    {
      get;
      set;
    }
    public decimal Payed
    {
      get;
      set;
    }
    public DateTime? PrintDate
    {
      get;
      set;
    }
    public string Remarks
    {
      get;
      set;
    }
    public bool Selector
    {
      get;
      set;
    }
    public decimal? Totaal
    {
      get;
      set;
    }
  }
}
