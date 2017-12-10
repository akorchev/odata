using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyApp.Controllers.Northwind
{
  using Models;
  using Data;
  using Models.Northwind;

  [EnableQuery]
  [ODataRoute("odata/Northwind/CustomerDemographics")]
  public partial class CustomerDemographicsController : Controller
  {
    private Data.NorthwindContext context;

    public CustomerDemographicsController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/CustomerDemographics
    [HttpGet]
    public IEnumerable<Models.Northwind.CustomerDemographic> Get()
    {
      var items = this.context.CustomerDemographics.AsQueryable<Models.Northwind.CustomerDemographic>();

      this.OnCustomerDemographicsRead(ref items);

      return items;
    }

    partial void OnCustomerDemographicsRead(ref IQueryable<Models.Northwind.CustomerDemographic> items);

    [HttpGet("{CustomerTypeID}")]
    public IActionResult GetCustomerDemographic(string key)
    {
        var item = this.context.CustomerDemographics.Where(i=>i.CustomerTypeID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnCustomerDemographicDeleted(Models.Northwind.CustomerDemographic item);

    [HttpDelete("{CustomerTypeID}")]
    public IActionResult DeleteCustomerDemographic(string key)
    {
        var item = this.context.CustomerDemographics
            .Where(i => i.CustomerTypeID == key)
            .Include(i => i.CustomerCustomerDemos)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCustomerDemographicDeleted(item);
        this.context.CustomerDemographics.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerDemographicUpdated(Models.Northwind.CustomerDemographic item);

    [HttpPut("{CustomerTypeID}")]
    public IActionResult PutCustomerDemographic(string key, [FromBody]Models.Northwind.CustomerDemographic newItem)
    {
        if (newItem == null || newItem.CustomerTypeID != key)
        {
            return BadRequest();
        }

        this.OnCustomerDemographicUpdated(newItem);
        this.context.CustomerDemographics.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CustomerTypeID}")]
    public IActionResult PatchCustomerDemographic(string key, [FromBody]JObject patch)
    {
        var item = this.context.CustomerDemographics.Where(i=>i.CustomerTypeID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnCustomerDemographicUpdated(item);
        this.context.CustomerDemographics.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerDemographicCreated(Models.Northwind.CustomerDemographic item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.CustomerDemographic item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCustomerDemographicCreated(item);
        this.context.CustomerDemographics.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/CustomerDemographics/{item.CustomerTypeID}", item);
    }
  }
}
