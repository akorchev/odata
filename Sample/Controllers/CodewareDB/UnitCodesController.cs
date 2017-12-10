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
  [ODataRoute("odata/CodewareDb/UnitCodes")]
  public partial class UnitCodesController : Controller
  {
    private CodewareDbContext context;

    public UnitCodesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/UnitCodes
    [HttpGet]
    public IEnumerable<UnitCode> Get()
    {
      var items = this.context.UnitCodes.AsQueryable<UnitCode>();

      this.OnUnitCodesRead(ref items);

      return items;
    }

    partial void OnUnitCodesRead(ref IQueryable<UnitCode> items);

    [HttpGet("{UnitCode1}")]
    public IActionResult GetUnitCode(string key)
    {
        var item = this.context.UnitCodes.Where(i=>i.UnitCode1 == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnUnitCodeDeleted(UnitCode item);

    [HttpDelete("{UnitCode1}")]
    public IActionResult DeleteUnitCode(string key)
    {
        var item = this.context.UnitCodes
            .Where(i => i.UnitCode1 == key)
            .Include(i => i.Details)
            .Include(i => i.Projects)
            .Include(i => i.Categories)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnUnitCodeDeleted(item);
        this.context.UnitCodes.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnUnitCodeUpdated(UnitCode item);

    [HttpPut("{UnitCode1}")]
    public IActionResult PutUnitCode(string key, [FromBody]UnitCode newItem)
    {
        if (newItem == null || newItem.UnitCode1 != key)
        {
            return BadRequest();
        }

        this.OnUnitCodeUpdated(newItem);
        this.context.UnitCodes.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{UnitCode1}")]
    public IActionResult PatchUnitCode(string key, [FromBody]JObject patch)
    {
        var item = this.context.UnitCodes.Where(i=>i.UnitCode1 == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnUnitCodeUpdated(item);
        this.context.UnitCodes.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnUnitCodeCreated(UnitCode item);

    [HttpPost]
    public IActionResult Post([FromBody] UnitCode item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnUnitCodeCreated(item);
        this.context.UnitCodes.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/UnitCodes/{item.UnitCode1}", item);
    }
  }
}
