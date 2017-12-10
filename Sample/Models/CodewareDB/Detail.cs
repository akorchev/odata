using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Details", Schema = "dbo")]
  public class Detail
  {
    public int? CategoryID
    {
      get;
      set;
    }

    [ForeignKey("CategoryID")]
    public Category Category { get; set; }
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
    public string Description
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DetailID
    {
      get;
      set;
    }


    [InverseProperty("Detail1")]
    public ICollection<Detail> Details { get; set; }
    public decimal Discount
    {
      get;
      set;
    }
    public DateTime EntryDate
    {
      get;
      set;
    }
    public DateTime? FromTime
    {
      get;
      set;
    }
    public int? InvoiceDetailID
    {
      get;
      set;
    }

    [ForeignKey("InvoiceDetailID")]
    public InvoiceDetail InvoiceDetail { get; set; }
    public int? InvoiceID
    {
      get;
      set;
    }

    [ForeignKey("InvoiceID")]
    public Invoice Invoice { get; set; }
    public int? KBID
    {
      get;
      set;
    }

    [ForeignKey("KBID")]
    public KnowledgeBase KnowledgeBase { get; set; }
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    public int? ParentDetailID
    {
      get;
      set;
    }

    [ForeignKey("ParentDetailID")]
    public Detail Detail1 { get; set; }
    public int? ProjectID
    {
      get;
      set;
    }

    [ForeignKey("ProjectID")]
    public Project Project { get; set; }
    public decimal PurchasePrice
    {
      get;
      set;
    }
    public decimal? PurchaseQuantity
    {
      get;
      set;
    }
    public string PurchaseUnit
    {
      get;
      set;
    }
    public decimal? Quantity
    {
      get;
      set;
    }
    public string Referentie
    {
      get;
      set;
    }
    public bool Selector
    {
      get;
      set;
    }
    public DateTime? ToTime
    {
      get;
      set;
    }
    public string Type
    {
      get;
      set;
    }

    [ForeignKey("Type")]
    public DetailType DetailType { get; set; }
    public string UnitCode
    {
      get;
      set;
    }

    [ForeignKey("UnitCode")]
    public UnitCode UnitCode1 { get; set; }
    public decimal UnitPrice
    {
      get;
      set;
    }
    public int UserID
    {
      get;
      set;
    }

    [ForeignKey("UserID")]
    public User User { get; set; }
    public string VatCode
    {
      get;
      set;
    }

    [ForeignKey("VatCode")]
    public VatCode VatCode1 { get; set; }
  }
}
