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
  [ODataRoutePrefix("odata/Northwind/Categories")]
  public partial class CategoriesController : Controller
  {
    private Data.NorthwindContext context;

    public CategoriesController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Categories
    [HttpGet]
    public IEnumerable<Models.Northwind.Category> Get()
    {
      var items = this.context.Categories.AsQueryable<Models.Northwind.Category>();

      this.OnCategoriesRead(ref items);

      return items;
    }

    partial void OnCategoriesRead(ref IQueryable<Models.Northwind.Category> items);

    [HttpGet("{CategoryID}")]
    public SingleResult<Models.Northwind.Category> GetCategory(int key)
    {
        var items = this.context.Categories.Where(i=>i.CategoryID == key);

        return SingleResult.Create(items);
    }
    partial void OnCategoryDeleted(Models.Northwind.Category item);

    [HttpDelete("{CategoryID}")]
    public IActionResult DeleteCategory(int key)
    {
        var item = this.context.Categories
            .Where(i => i.CategoryID == key)
            .Include(i => i.Products)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCategoryDeleted(item);
        this.context.Categories.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCategoryUpdated(Models.Northwind.Category item);

    [HttpPut("{CategoryID}")]
    public IActionResult PutCategory(int key, [FromBody]Models.Northwind.Category newItem)
    {
        if (newItem == null || newItem.CategoryID != key)
        {
            return BadRequest();
        }

        this.OnCategoryUpdated(newItem);
        this.context.Categories.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CategoryID}")]
    public IActionResult PatchCategory(int key, [FromBody]JObject patch)
    {
        var item = this.context.Categories.Where(i=>i.CategoryID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnCategoryUpdated(item);
        this.context.Categories.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCategoryCreated(Models.Northwind.Category item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Category item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCategoryCreated(item);
        this.context.Categories.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Categories/{item.CategoryID}", item);
    }
  }
}
