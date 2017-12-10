using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Customers", Schema = "dbo")]
  public class Customer
  {
    public bool Active
    {
      get;
      set;
    }
    public string Adres
    {
      get;
      set;
    }
    public string City
    {
      get;
      set;
    }
    public string ContactFirstName
    {
      get;
      set;
    }
    public string CountryCode
    {
      get;
      set;
    }

    [ForeignKey("CountryCode")]
    public Country Country { get; set; }
    public DateTime? CreationDate
    {
      get;
      set;
    }
    [Key]
    public string CustomerCode
    {
      get;
      set;
    }


    [InverseProperty("Customer")]
    public ICollection<CustomerKnowledgeBase> CustomerKnowledgeBases { get; set; }

    [InverseProperty("Customer")]
    public ICollection<Project> Projects { get; set; }

    [InverseProperty("Customer")]
    public ICollection<SupportIssue> SupportIssues { get; set; }

    [InverseProperty("Customer")]
    public ICollection<Invoice> Invoices { get; set; }

    [InverseProperty("Customer")]
    public ICollection<SupportCategory> SupportCategories { get; set; }

    [InverseProperty("Customer")]
    public ICollection<User> Users { get; set; }

    [InverseProperty("Customer")]
    public ICollection<CustomerContact> CustomerContacts { get; set; }
    public string DefaultVATCode
    {
      get;
      set;
    }

    [ForeignKey("DefaultVATCode")]
    public VatCode VatCode { get; set; }
    public decimal? Discount
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
    public DateTime? LastEditDate
    {
      get;
      set;
    }
    public string MailBodyFactuur
    {
      get;
      set;
    }
    public string Name
    {
      get;
      set;
    }
    public string Phone
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
    public string VATNumber
    {
      get;
      set;
    }
    public string Zipcode
    {
      get;
      set;
    }
  }
}
