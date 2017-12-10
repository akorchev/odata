using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodewareDb.Models.CodewareDb
{
  [Table("Products", Schema = "dbo")]
  public class Product
  {
    public string Name
    {
      get;
      set;
    }
    public double Price
    {
      get;
      set;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID
    {
      get;
      set;
    }
    public string UrlWithInfo
    {
      get;
      set;
    }
    public bool WebProduct
    {
      get;
      set;
    }
  }
}
