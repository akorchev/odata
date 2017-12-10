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
  [ODataRoute("odata/Northwind/Shippers")]
  public partial class ShippersController : Controller
  {
    private Data.NorthwindContext context;

    public ShippersController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Shippers
    [HttpGet]
    public IEnumerable<Models.Northwind.Shipper> Get()
    {
      var items = this.context.Shippers.AsQueryable<Models.Northwind.Shipper>();

      this.OnShippersRead(ref items);

      return items;
    }

    partial void OnShippersRead(ref IQueryable<Models.Northwind.Shipper> items);

    [HttpGet("{ShipperID}")]
    public IActionResult GetShipper(int key)
    {
        var item = this.context.Shippers.Where(i=>i.ShipperID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnShipperDeleted(Models.Northwind.Shipper item);

    [HttpDelete("{ShipperID}")]
    public IActionResult DeleteShipper(int key)
    {
        var item = this.context.Shippers
            .Where(i => i.ShipperID == key)
            .Include(i => i.Orders)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnShipperDeleted(item);
        this.context.Shippers.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnShipperUpdated(Models.Northwind.Shipper item);

    [HttpPut("{ShipperID}")]
    public IActionResult PutShipper(int key, [FromBody]Models.Northwind.Shipper newItem)
    {
        if (newItem == null || newItem.ShipperID != key)
        {
            return BadRequest();
        }

        this.OnShipperUpdated(newItem);
        this.context.Shippers.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ShipperID}")]
    public IActionResult PatchShipper(int key, [FromBody]JObject patch)
    {
        var item = this.context.Shippers.Where(i=>i.ShipperID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnShipperUpdated(item);
        this.context.Shippers.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnShipperCreated(Models.Northwind.Shipper item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Shipper item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnShipperCreated(item);
        this.context.Shippers.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Shippers/{item.ShipperID}", item);
    }
  }
}
