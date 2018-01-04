using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models.Sample
{
  [Table("Orders", Schema = "dbo")]
  public class Order
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }


    [InverseProperty("Order")]
    public ICollection<OrderDetail> OrderDetails { get; set; }
    public DateTime OrderDate
    {
      get;
      set;
    }
    public string UserName
    {
      get;
      set;
    }
  }
}
