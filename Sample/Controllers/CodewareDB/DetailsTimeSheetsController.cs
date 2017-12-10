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
  [ODataRoute("odata/CodewareDb/DetailsTimeSheets")]
  public partial class DetailsTimeSheetsController : Controller
  {
    private CodewareDbContext context;

    public DetailsTimeSheetsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/DetailsTimeSheets
    [HttpGet]
    public IEnumerable<DetailsTimeSheet> Get()
    {
      var items = this.context.DetailsTimeSheets.AsQueryable<DetailsTimeSheet>();

      this.OnDetailsTimeSheetsRead(ref items);

      return items;
    }

    partial void OnDetailsTimeSheetsRead(ref IQueryable<DetailsTimeSheet> items);

    [HttpGet("{DetailHelperID}")]
    public IActionResult GetDetailsTimeSheet(int key)
    {
        var item = this.context.DetailsTimeSheets.Where(i=>i.DetailHelperID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnDetailsTimeSheetDeleted(DetailsTimeSheet item);

    [HttpDelete("{DetailHelperID}")]
    public IActionResult DeleteDetailsTimeSheet(int key)
    {
        var item = this.context.DetailsTimeSheets
            .Where(i => i.DetailHelperID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnDetailsTimeSheetDeleted(item);
        this.context.DetailsTimeSheets.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDetailsTimeSheetUpdated(DetailsTimeSheet item);

    [HttpPut("{DetailHelperID}")]
    public IActionResult PutDetailsTimeSheet(int key, [FromBody]DetailsTimeSheet newItem)
    {
        if (newItem == null || newItem.DetailHelperID != key)
        {
            return BadRequest();
        }

        this.OnDetailsTimeSheetUpdated(newItem);
        this.context.DetailsTimeSheets.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{DetailHelperID}")]
    public IActionResult PatchDetailsTimeSheet(int key, [FromBody]JObject patch)
    {
        var item = this.context.DetailsTimeSheets.Where(i=>i.DetailHelperID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnDetailsTimeSheetUpdated(item);
        this.context.DetailsTimeSheets.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDetailsTimeSheetCreated(DetailsTimeSheet item);

    [HttpPost]
    public IActionResult Post([FromBody] DetailsTimeSheet item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnDetailsTimeSheetCreated(item);
        this.context.DetailsTimeSheets.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/DetailsTimeSheets/{item.DetailHelperID}", item);
    }
  }
}
