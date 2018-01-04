using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyApp.Controllers.Northwind
{
  using Models;
  using Data;
  using Models.Northwind;

  [EnableQuery]
  [ODataRoutePrefix("odata/Northwind/Customers")]
  public partial class CustomersController : Controller
  {
    private Data.NorthwindContext context;

    public CustomersController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Customers
    [HttpGet]
    public IEnumerable<Models.Northwind.Customer> Get()
    {
      var items = this.context.Customers.AsQueryable();

      this.OnCustomersRead(ref items);

      return items;
    }

    partial void OnCustomersRead(ref IQueryable<Models.Northwind.Customer> items);

    [HttpGet("{CustomerID}")]
    public SingleResult<Models.Northwind.Customer> GetCustomer(string key)
    {
            var items = this.context.Customers.Where(i => i.CustomerID == key);

            return SingleResult.Create(items);
        }
    partial void OnCustomerDeleted(Models.Northwind.Customer item);

    [HttpDelete("{CustomerID}")]
    public IActionResult DeleteCustomer(string key)
    {
        var item = this.context.Customers
            .Where(i => i.CustomerID == key)
            .Include(i => i.Orders)
            .Include(i => i.CustomerCustomerDemos)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCustomerDeleted(item);
        this.context.Customers.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerUpdated(Models.Northwind.Customer item);

    [HttpPut("{CustomerID}")]
    public IActionResult PutCustomer(string key, [FromBody]Models.Northwind.Customer newItem)
    {
        if (newItem == null || newItem.CustomerID != key)
        {
            return BadRequest();
        }

        this.OnCustomerUpdated(newItem);
        this.context.Customers.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CustomerID}")]
    public IActionResult PatchCustomer(string key, [FromBody]JObject patch)
    {
        var item = this.context.Customers.Where(i=>i.CustomerID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnCustomerUpdated(item);
        this.context.Customers.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerCreated(Models.Northwind.Customer item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Customer item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCustomerCreated(item);
        this.context.Customers.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Customers/{item.CustomerID}", item);
    }
  }
}
