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
  [ODataRoute("odata/Northwind/Products")]
  public partial class ProductsController : Controller
  {
    private Data.NorthwindContext context;

    public ProductsController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Products
    [HttpGet]
    public IEnumerable<Models.Northwind.Product> Get()
    {
      var items = this.context.Products.AsQueryable<Models.Northwind.Product>();

      this.OnProductsRead(ref items);

      return items;
    }

    partial void OnProductsRead(ref IQueryable<Models.Northwind.Product> items);

    [HttpGet("{ProductID}")]
    public IActionResult GetProduct(int key)
    {
        var item = this.context.Products.Where(i=>i.ProductID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnProductDeleted(Models.Northwind.Product item);

    [HttpDelete("{ProductID}")]
    public IActionResult DeleteProduct(int key)
    {
        var item = this.context.Products
            .Where(i => i.ProductID == key)
            .Include(i => i.OrderDetails)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnProductDeleted(item);
        this.context.Products.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProductUpdated(Models.Northwind.Product item);

    [HttpPut("{ProductID}")]
    public IActionResult PutProduct(int key, [FromBody]Models.Northwind.Product newItem)
    {
        if (newItem == null || newItem.ProductID != key)
        {
            return BadRequest();
        }

        this.OnProductUpdated(newItem);
        this.context.Products.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ProductID}")]
    public IActionResult PatchProduct(int key, [FromBody]JObject patch)
    {
        var item = this.context.Products.Where(i=>i.ProductID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnProductUpdated(item);
        this.context.Products.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProductCreated(Models.Northwind.Product item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Product item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProductCreated(item);
        this.context.Products.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Products/{item.ProductID}", item);
    }
  }
}
