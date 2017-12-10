using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models.Northwind
{
  [Table("Order Details", Schema = "dbo")]
  public class OrderDetail
  {
    public float Discount
    {
      get;
      set;
    }
    [Key]
    public int OrderID
    {
      get;
      set;
    }

    [ForeignKey("OrderID")]
    public Order Order { get; set; }
    [Key]
    public int ProductID
    {
      get;
      set;
    }

    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public Int16 Quantity
    {
      get;
      set;
    }
    public decimal UnitPrice
    {
      get;
      set;
    }
  }
}
