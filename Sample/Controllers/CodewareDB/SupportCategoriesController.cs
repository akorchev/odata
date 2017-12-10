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
  [ODataRoute("odata/CodewareDb/SupportCategories")]
  public partial class SupportCategoriesController : Controller
  {
    private CodewareDbContext context;

    public SupportCategoriesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/SupportCategories
    [HttpGet]
    public IEnumerable<SupportCategory> Get()
    {
      var items = this.context.SupportCategories.AsQueryable<SupportCategory>();

      this.OnSupportCategoriesRead(ref items);

      return items;
    }

    partial void OnSupportCategoriesRead(ref IQueryable<SupportCategory> items);

    [HttpGet("{SupportCategoryID}")]
    public IActionResult GetSupportCategory(int key)
    {
        var item = this.context.SupportCategories.Where(i=>i.SupportCategoryID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnSupportCategoryDeleted(SupportCategory item);

    [HttpDelete("{SupportCategoryID}")]
    public IActionResult DeleteSupportCategory(int key)
    {
        var item = this.context.SupportCategories
            .Where(i => i.SupportCategoryID == key)
            .Include(i => i.SupportIssues)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnSupportCategoryDeleted(item);
        this.context.SupportCategories.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportCategoryUpdated(SupportCategory item);

    [HttpPut("{SupportCategoryID}")]
    public IActionResult PutSupportCategory(int key, [FromBody]SupportCategory newItem)
    {
        if (newItem == null || newItem.SupportCategoryID != key)
        {
            return BadRequest();
        }

        this.OnSupportCategoryUpdated(newItem);
        this.context.SupportCategories.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{SupportCategoryID}")]
    public IActionResult PatchSupportCategory(int key, [FromBody]JObject patch)
    {
        var item = this.context.SupportCategories.Where(i=>i.SupportCategoryID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnSupportCategoryUpdated(item);
        this.context.SupportCategories.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportCategoryCreated(SupportCategory item);

    [HttpPost]
    public IActionResult Post([FromBody] SupportCategory item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnSupportCategoryCreated(item);
        this.context.SupportCategories.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/SupportCategories/{item.SupportCategoryID}", item);
    }
  }
}
