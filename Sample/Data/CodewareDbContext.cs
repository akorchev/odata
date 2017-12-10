using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using CodewareDb.Models.CodewareDb;

namespace CodewareDb.Data
{
  public partial class CodewareDbContext : DbContext
  {
    public CodewareDbContext(DbContextOptions<CodewareDbContext> options):base(options)
    {
    }

    public CodewareDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProjectMember>().HasKey(table => new {
          table.ProjectID, table.UserID
        });

        builder.Entity<RolePermission>().HasKey(table => new {
          table.PermissionId, table.RoleName
        });

        builder.Entity<RolePermissionSet>().HasKey(table => new {
          table.PermissionId, table.RoleName
        });

    }


    public DbSet<Application> Applications
    {
      get;
      set;
    }

    public DbSet<Category> Categories
    {
      get;
      set;
    }

    public DbSet<Company> Companies
    {
      get;
      set;
    }

    public DbSet<Country> Countries
    {
      get;
      set;
    }

    public DbSet<Customer> Customers
    {
      get;
      set;
    }

    public DbSet<CustomerContact> CustomerContacts
    {
      get;
      set;
    }

    public DbSet<CustomerKnowledgeBase> CustomerKnowledgeBases
    {
      get;
      set;
    }

    public DbSet<DatePeriod> DatePeriods
    {
      get;
      set;
    }

    public DbSet<DateSelector> DateSelectors
    {
      get;
      set;
    }

    public DbSet<Detail> Details
    {
      get;
      set;
    }

    public DbSet<DetailType> DetailTypes
    {
      get;
      set;
    }

    public DbSet<DetailsTimeSheet> DetailsTimeSheets
    {
      get;
      set;
    }

    public DbSet<DropdownList> DropdownLists
    {
      get;
      set;
    }

    public DbSet<Invoice> Invoices
    {
      get;
      set;
    }

    public DbSet<InvoiceDetail> InvoiceDetails
    {
      get;
      set;
    }

    public DbSet<KnowledegeBaseSubType> KnowledegeBaseSubTypes
    {
      get;
      set;
    }

    public DbSet<KnowledgeBase> KnowledgeBases
    {
      get;
      set;
    }

    public DbSet<KnowledgeBaseScriptInit> KnowledgeBaseScriptInits
    {
      get;
      set;
    }

    public DbSet<KnowledgeBaseScriptType> KnowledgeBaseScriptTypes
    {
      get;
      set;
    }

    public DbSet<KnowledgeBaseType> KnowledgeBaseTypes
    {
      get;
      set;
    }

    public DbSet<Mail> Mails
    {
      get;
      set;
    }

    public DbSet<Membership> Memberships
    {
      get;
      set;
    }

    public DbSet<Product> Products
    {
      get;
      set;
    }

    public DbSet<Profile> Profiles
    {
      get;
      set;
    }

    public DbSet<Project> Projects
    {
      get;
      set;
    }

    public DbSet<ProjectCalculation> ProjectCalculations
    {
      get;
      set;
    }

    public DbSet<ProjectMember> ProjectMembers
    {
      get;
      set;
    }

    public DbSet<ProjectStaffel> ProjectStaffels
    {
      get;
      set;
    }

    public DbSet<Role> Roles
    {
      get;
      set;
    }

    public DbSet<RolePermission> RolePermissions
    {
      get;
      set;
    }

    public DbSet<RolePermissionSet> RolePermissionSets
    {
      get;
      set;
    }

    public DbSet<SupportCategory> SupportCategories
    {
      get;
      set;
    }

    public DbSet<SupportIssue> SupportIssues
    {
      get;
      set;
    }

    public DbSet<SupportPriority> SupportPriorities
    {
      get;
      set;
    }

    public DbSet<SupportState> SupportStates
    {
      get;
      set;
    }

    public DbSet<Unit> Units
    {
      get;
      set;
    }

    public DbSet<UnitCode> UnitCodes
    {
      get;
      set;
    }

    public DbSet<User> Users
    {
      get;
      set;
    }

    public DbSet<VatCode> VatCodes
    {
      get;
      set;
    }
  }
}
