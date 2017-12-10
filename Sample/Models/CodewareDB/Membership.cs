using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Memberships", Schema = "dbo")]
  public class Membership
  {
    public Guid ApplicationId
    {
      get;
      set;
    }
    public string Comment
    {
      get;
      set;
    }
    public DateTime CreateDate
    {
      get;
      set;
    }
    public string Email
    {
      get;
      set;
    }
    public int FailedPasswordAnswerAttemptCount
    {
      get;
      set;
    }
    public DateTime FailedPasswordAnswerAttemptWindowsStart
    {
      get;
      set;
    }
    public int FailedPasswordAttemptCount
    {
      get;
      set;
    }
    public DateTime FailedPasswordAttemptWindowStart
    {
      get;
      set;
    }
    public bool IsApproved
    {
      get;
      set;
    }
    public bool IsLockedOut
    {
      get;
      set;
    }
    public DateTime LastLockoutDate
    {
      get;
      set;
    }
    public DateTime LastLoginDate
    {
      get;
      set;
    }
    public DateTime LastPasswordChangedDate
    {
      get;
      set;
    }
    public string Password
    {
      get;
      set;
    }
    public string PasswordAnswer
    {
      get;
      set;
    }
    public int PasswordFormat
    {
      get;
      set;
    }
    public string PasswordQuestion
    {
      get;
      set;
    }
    public string PasswordSalt
    {
      get;
      set;
    }
    [Key]
    public Guid UserId
    {
      get;
      set;
    }
  }
}
