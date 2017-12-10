using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using MyApp.Models.Northwind;

namespace MyApp.Data
{
  public partial class NorthwindContext : DbContext
  {
    public NorthwindContext(DbContextOptions<NorthwindContext> options):base(options)
    {
    }

    public NorthwindContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CustomerCustomerDemo>().HasKey(table => new {
          table.CustomerID, table.CustomerTypeID
        });

        builder.Entity<EmployeeTerritory>().HasKey(table => new {
          table.EmployeeID, table.TerritoryID
        });

        builder.Entity<OrderDetail>().HasKey(table => new {
          table.OrderID, table.ProductID
        });

    }


    public DbSet<Category> Categories
    {
      get;
      set;
    }

    public DbSet<Customer> Customers
    {
      get;
      set;
    }

    public DbSet<CustomerCustomerDemo> CustomerCustomerDemos
    {
      get;
      set;
    }

    public DbSet<CustomerDemographic> CustomerDemographics
    {
      get;
      set;
    }

    public DbSet<Employee> Employees
    {
      get;
      set;
    }

    public DbSet<EmployeeTerritory> EmployeeTerritories
    {
      get;
      set;
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

    public DbSet<Region> Regions
    {
      get;
      set;
    }

    public DbSet<Shipper> Shippers
    {
      get;
      set;
    }

    public DbSet<Supplier> Suppliers
    {
      get;
      set;
    }

    public DbSet<Territory> Territories
    {
      get;
      set;
    }
  }
}
