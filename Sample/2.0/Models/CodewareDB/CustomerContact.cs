using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("CustomerContacts", Schema = "dbo")]
  public class CustomerContact
  {
    public bool Active
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactID
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
    public string Mobile
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
    public string eMail
    {
      get;
      set;
    }
  }
}
