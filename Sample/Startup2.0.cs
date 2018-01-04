using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using CodewareDb.Models.CodewareDb;
using CodewareDb.Data;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;

namespace Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();
            services.AddODataQueryFilter();
            /*
            string connectionString = "Server=localhost;Initial Catalog=CodewareDB;Persist Security Info=False;User ID=sa;Password=passw0rdMSSQL;MultipleActiveResultSets=False;Encrypt=false;TrustServerCertificate=true;Connection Timeout=30";

            services.AddDbContext<CodewareDbContext>(options =>
                            options.UseSqlServer(connectionString));*/
            string connectionString = "Server=localhost;Initial Catalog=Northwind;Persist Security Info=False;User ID=sa;Password=passw0rdMSSQL;MultipleActiveResultSets=False;Encrypt=false;TrustServerCertificate=true;Connection Timeout=30";

            services.AddDbContext<MyApp.Data.NorthwindContext>(options =>
                            options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env)
        {
            IServiceProvider provider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

            //app.UseMvc(builder => builder.MapODataServiceRoute("odata", "odata/CodewareDb", GetCodewareDbEdmModel(provider)));
            app.UseMvc(builder => {
                builder.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                builder.MapODataServiceRoute("odata", "odata/Northwind", GetNorthwindEdmModel(provider));
            });
        }

        private static IEdmModel GetCodewareDbEdmModel(IServiceProvider provider)
        {
            var builder = new ODataConventionModelBuilder(provider);
            builder.ContainerName = "CodewareDbContext";

            builder.EntitySet<Detail>("Details");
            builder.EntitySet<Company>("Companies");
            builder.EntitySet<Project>("Projects");

            return builder.GetEdmModel();
        }

        private static IEdmModel GetNorthwindEdmModel(IServiceProvider provider)
        {
            var northwindBuilder = new ODataConventionModelBuilder(provider);
            northwindBuilder.ContainerName = "NorthwindContext";

            northwindBuilder.EntitySet<MyApp.Models.Northwind.Category>("Categories");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Customer>("Customers");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.CustomerCustomerDemo>("CustomerCustomerDemos");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.CustomerDemographic>("CustomerDemographics");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Employee>("Employees");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.EmployeeTerritory>("EmployeeTerritories");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Order>("Orders");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.OrderDetail>("OrderDetails");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Product>("Products");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Region>("Regions");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Shipper>("Shippers");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Supplier>("Suppliers");
            northwindBuilder.EntitySet<MyApp.Models.Northwind.Territory>("Territories");

            return northwindBuilder.GetEdmModel();
        }
    }
}
