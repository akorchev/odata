using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CodewareDb.Models;
using CodewareDb.Data;
using CodewareDb.Models.CodewareDb;

namespace CodewareDb.Controllers.CodewareDb
{
  [EnableQuery]
  [ODataRoute("odata/CodewareDb/Products")]
  public partial class ProductsController : Controller
  {
    private CodewareDbContext context;

    public ProductsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Products
    [HttpGet]
    public IEnumerable<Product> Get()
    {
      var items = this.context.Products.AsQueryable<Product>();

      this.OnProductsRead(ref items);

      return items;
    }

    partial void OnProductsRead(ref IQueryable<Product> items);

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
    partial void OnProductDeleted(Product item);

    [HttpDelete("{ProductID}")]
    public IActionResult DeleteProduct(int key)
    {
        var item = this.context.Products
            .Where(i => i.ProductID == key)
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

    partial void OnProductUpdated(Product item);

    [HttpPut("{ProductID}")]
    public IActionResult PutProduct(int key, [FromBody]Product newItem)
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

        EntityPatch.Apply(item, patch);

        this.OnProductUpdated(item);
        this.context.Products.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProductCreated(Product item);

    [HttpPost]
    public IActionResult Post([FromBody] Product item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProductCreated(item);
        this.context.Products.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Products/{item.ProductID}", item);
    }
  }
}
