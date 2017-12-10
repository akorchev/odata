using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Builder;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.OData.Edm;

namespace Sample
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddOData();

            // Add framework services.
            //      services.AddMvc().AddJsonOptions(options => {
            //         options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //        });

            //services.AddSingleton<NorthwindContext>();
            string connectionString = "Server=localhost;Initial Catalog=Northwind;Persist Security Info=False;User ID=sa;Password=passw0rdMSSQL;MultipleActiveResultSets=False;Encrypt=false;TrustServerCertificate=true;Connection Timeout=30";

            services.AddDbContext<MyApp.Data.NorthwindContext>(options =>
                            options.UseSqlServer(connectionString));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      IAssemblyProvider provider = app.ApplicationServices.GetRequiredService<IAssemblyProvider>();
      IEdmModel model = GetEdmModel(provider);

      // Single
      app.UseMvc(builder => builder.MapODataRoute("odata/Northwind", model));

    }

    private static IEdmModel GetEdmModel(IAssemblyProvider provider)
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
