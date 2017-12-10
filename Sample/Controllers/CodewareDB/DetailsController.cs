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
  [ODataRoute("odata/CodewareDb/Details")]
  public partial class DetailsController : Controller
  {
    private CodewareDbContext context;

    public DetailsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Details
    [HttpGet]
    public IEnumerable<Detail> Get()
    {
      var items = this.context.Details.AsQueryable<Detail>();

      this.OnDetailsRead(ref items);

      return items;
    }

    partial void OnDetailsRead(ref IQueryable<Detail> items);

    [HttpGet("{DetailID}")]
    public IActionResult GetDetail(int key)
    {
        var item = this.context.Details.Where(i=>i.DetailID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnDetailDeleted(Detail item);

    [HttpDelete("{DetailID}")]
    public IActionResult DeleteDetail(int key)
    {
        var item = this.context.Details
            .Where(i => i.DetailID == key)
            .Include(i => i.Details)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnDetailDeleted(item);
        this.context.Details.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDetailUpdated(Detail item);

    [HttpPut("{DetailID}")]
    public IActionResult PutDetail(int key, [FromBody]Detail newItem)
    {
        if (newItem == null || newItem.DetailID != key)
        {
            return BadRequest();
        }

        this.OnDetailUpdated(newItem);
        this.context.Details.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{DetailID}")]
    public IActionResult PatchDetail(int key, [FromBody]JObject patch)
    {
        var item = this.context.Details.Where(i=>i.DetailID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnDetailUpdated(item);
        this.context.Details.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDetailCreated(Detail item);

    [HttpPost]
    public IActionResult Post([FromBody] Detail item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnDetailCreated(item);
        this.context.Details.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Details/{item.DetailID}", item);
    }
  }
}
