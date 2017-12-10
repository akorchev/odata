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
  [ODataRoute("odata/CodewareDb/Categories")]
  public partial class CategoriesController : Controller
  {
    private CodewareDbContext context;

    public CategoriesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Categories
    [HttpGet]
    public IEnumerable<Category> Get()
    {
      var items = this.context.Categories.AsQueryable<Category>();

      this.OnCategoriesRead(ref items);

      return items;
    }

    partial void OnCategoriesRead(ref IQueryable<Category> items);

    [HttpGet("{CategoryID}")]
    public IActionResult GetCategory(int key)
    {
        var item = this.context.Categories.Where(i=>i.CategoryID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnCategoryDeleted(Category item);

    [HttpDelete("{CategoryID}")]
    public IActionResult DeleteCategory(int key)
    {
        var item = this.context.Categories
            .Where(i => i.CategoryID == key)
            .Include(i => i.Details)
            .Include(i => i.Projects)
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

    partial void OnCategoryUpdated(Category item);

    [HttpPut("{CategoryID}")]
    public IActionResult PutCategory(int key, [FromBody]Category newItem)
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

        EntityPatch.Apply(item, patch);

        this.OnCategoryUpdated(item);
        this.context.Categories.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCategoryCreated(Category item);

    [HttpPost]
    public IActionResult Post([FromBody] Category item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCategoryCreated(item);
        this.context.Categories.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Categories/{item.CategoryID}", item);
    }
  }
}
