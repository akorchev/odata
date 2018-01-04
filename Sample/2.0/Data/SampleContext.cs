using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using MyApp.Models.Sample;

namespace MyApp.Data
{
  public partial class SampleContext : DbContext
  {
    public SampleContext(DbContextOptions<SampleContext> options):base(options)
    {
    }

    public SampleContext()
    {
    }



    public DbSet<Order> Orders
    {
      get;
      set;
    }

    public DbSet<OrderDetail> OrderDetails
    {
      get;
      set;
    }

    public DbSet<Product> Products
    {
      get;
      set;
    }
  }
}
