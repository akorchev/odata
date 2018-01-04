using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyApp.Controllers.Sample
{
  using Models;
  using Data;
  using Models.Sample;

  [EnableQuery]
  [ODataRoutePrefix("odata/Sample/Products")]
  public partial class ProductsController : Controller
  {
    private Data.SampleContext context;

    public ProductsController(Data.SampleContext context)
    {
      this.context = context;
    }
    // GET /odata/Sample/Products
    [HttpGet]
    public IEnumerable<Models.Sample.Product> GetProducts()
    {
      var items = this.context.Products.AsQueryable<Models.Sample.Product>();

      this.OnProductsRead(ref items);

      return items;
    }

    partial void OnProductsRead(ref IQueryable<Models.Sample.Product> items);

    [HttpGet("{Id}")]
    public IActionResult GetProduct(int key)
    {
        var item = this.context.Products.Where(i=>i.Id == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnProductDeleted(Models.Sample.Product item);

    [HttpDelete("{Id}")]
    public IActionResult DeleteProduct(int key)
    {
        var item = this.context.Products
            .Where(i => i.Id == key)
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

    partial void OnProductUpdated(Models.Sample.Product item);

    [HttpPut("{Id}")]
    public IActionResult PutProduct(int key, [FromBody]Models.Sample.Product newItem)
    {
        if (newItem == null || newItem.Id != key)
        {
            return BadRequest();
        }

        this.OnProductUpdated(newItem);
        this.context.Products.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Id}")]
    public IActionResult PatchProduct(int key, [FromBody]JObject patch)
    {
        var item = this.context.Products.Where(i=>i.Id == key).FirstOrDefault();

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

    partial void OnProductCreated(Models.Sample.Product item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Sample.Product item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProductCreated(item);
        this.context.Products.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Sample/Products/{item.Id}", item);
    }
  }
}
