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
  [ODataRoutePrefix("odata/Northwind/CustomerCustomerDemos")]
  public partial class CustomerCustomerDemosController : Controller
  {
    private Data.NorthwindContext context;

    public CustomerCustomerDemosController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/CustomerCustomerDemos
    [HttpGet]
    public IEnumerable<Models.Northwind.CustomerCustomerDemo> Get()
    {
      var items = this.context.CustomerCustomerDemos.AsQueryable<Models.Northwind.CustomerCustomerDemo>();

      this.OnCustomerCustomerDemosRead(ref items);

      return items;
    }

    partial void OnCustomerCustomerDemosRead(ref IQueryable<Models.Northwind.CustomerCustomerDemo> items);

    [HttpGet("{CustomerID}")]
    public SingleResult<Models.Northwind.CustomerCustomerDemo> GetCustomerCustomerDemo(string key)
    {
        var items = this.context.CustomerCustomerDemos.Where(i=>i.CustomerID == key);

       return SingleResult.Create(items);
        }
    partial void OnCustomerCustomerDemoDeleted(Models.Northwind.CustomerCustomerDemo item);

    [HttpDelete("{CustomerID}")]
    public IActionResult DeleteCustomerCustomerDemo(string key)
    {
        var item = this.context.CustomerCustomerDemos
            .Where(i => i.CustomerID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCustomerCustomerDemoDeleted(item);
        this.context.CustomerCustomerDemos.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerCustomerDemoUpdated(Models.Northwind.CustomerCustomerDemo item);

    [HttpPut("{CustomerID}")]
    public IActionResult PutCustomerCustomerDemo(string key, [FromBody]Models.Northwind.CustomerCustomerDemo newItem)
    {
        if (newItem == null || newItem.CustomerID != key)
        {
            return BadRequest();
        }

        this.OnCustomerCustomerDemoUpdated(newItem);
        this.context.CustomerCustomerDemos.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CustomerID}")]
    public IActionResult PatchCustomerCustomerDemo(string key, [FromBody]JObject patch)
    {
        var item = this.context.CustomerCustomerDemos.Where(i=>i.CustomerID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnCustomerCustomerDemoUpdated(item);
        this.context.CustomerCustomerDemos.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerCustomerDemoCreated(Models.Northwind.CustomerCustomerDemo item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.CustomerCustomerDemo item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCustomerCustomerDemoCreated(item);
        this.context.CustomerCustomerDemos.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/CustomerCustomerDemos/{item.CustomerID}", item);
    }
  }
}
