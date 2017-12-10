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
  [ODataRoutePrefix("odata/Northwind/Suppliers")]
  public partial class SuppliersController : Controller
  {
    private Data.NorthwindContext context;

    public SuppliersController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Suppliers
    [HttpGet]
    public IEnumerable<Models.Northwind.Supplier> Get()
    {
      var items = this.context.Suppliers.AsQueryable<Models.Northwind.Supplier>();

      this.OnSuppliersRead(ref items);

      return items;
    }

    partial void OnSuppliersRead(ref IQueryable<Models.Northwind.Supplier> items);

    [HttpGet("{SupplierID}")]
    public SingleResult<Models.Northwind.Supplier> GetSupplier(int key)
    {
        var items = this.context.Suppliers.Where(i=>i.SupplierID == key);

            return SingleResult.Create(items);
        }
    partial void OnSupplierDeleted(Models.Northwind.Supplier item);

    [HttpDelete("{SupplierID}")]
    public IActionResult DeleteSupplier(int key)
    {
        var item = this.context.Suppliers
            .Where(i => i.SupplierID == key)
            .Include(i => i.Products)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnSupplierDeleted(item);
        this.context.Suppliers.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupplierUpdated(Models.Northwind.Supplier item);

    [HttpPut("{SupplierID}")]
    public IActionResult PutSupplier(int key, [FromBody]Models.Northwind.Supplier newItem)
    {
        if (newItem == null || newItem.SupplierID != key)
        {
            return BadRequest();
        }

        this.OnSupplierUpdated(newItem);
        this.context.Suppliers.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{SupplierID}")]
    public IActionResult PatchSupplier(int key, [FromBody]JObject patch)
    {
        var item = this.context.Suppliers.Where(i=>i.SupplierID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnSupplierUpdated(item);
        this.context.Suppliers.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupplierCreated(Models.Northwind.Supplier item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Supplier item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnSupplierCreated(item);
        this.context.Suppliers.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Suppliers/{item.SupplierID}", item);
    }
  }
}
