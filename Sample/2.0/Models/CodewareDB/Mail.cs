using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Mails", Schema = "dbo")]
  public class Mail
  {
    public string Body
    {
      get;
      set;
    }
    public string FailedReason
    {
      get;
      set;
    }
    public string MailFrom
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MailID
    {
      get;
      set;
    }
    public string MailTo
    {
      get;
      set;
    }
    public string Parameters
    {
      get;
      set;
    }
    public string ReportName
    {
      get;
      set;
    }
    public bool Send
    {
      get;
      set;
    }
    public string Subject
    {
      get;
      set;
    }
    public DateTime TimeStamp
    {
      get;
      set;
    }
  }
}
