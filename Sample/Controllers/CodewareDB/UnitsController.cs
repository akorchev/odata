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
  [ODataRoute("odata/CodewareDb/Units")]
  public partial class UnitsController : Controller
  {
    private CodewareDbContext context;

    public UnitsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Units
    [HttpGet]
    public IEnumerable<Unit> Get()
    {
      var items = this.context.Units.AsQueryable<Unit>();

      this.OnUnitsRead(ref items);

      return items;
    }

    partial void OnUnitsRead(ref IQueryable<Unit> items);

    [HttpGet("{PK_Days}")]
    public IActionResult GetUnit(string key)
    {
        var item = this.context.Units.Where(i=>i.PK_Days == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnUnitDeleted(Unit item);

    [HttpDelete("{PK_Days}")]
    public IActionResult DeleteUnit(string key)
    {
        var item = this.context.Units
            .Where(i => i.PK_Days == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnUnitDeleted(item);
        this.context.Units.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnUnitUpdated(Unit item);

    [HttpPut("{PK_Days}")]
    public IActionResult PutUnit(string key, [FromBody]Unit newItem)
    {
        if (newItem == null || newItem.PK_Days != key)
        {
            return BadRequest();
        }

        this.OnUnitUpdated(newItem);
        this.context.Units.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{PK_Days}")]
    public IActionResult PatchUnit(string key, [FromBody]JObject patch)
    {
        var item = this.context.Units.Where(i=>i.PK_Days == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnUnitUpdated(item);
        this.context.Units.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnUnitCreated(Unit item);

    [HttpPost]
    public IActionResult Post([FromBody] Unit item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnUnitCreated(item);
        this.context.Units.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Units/{item.PK_Days}", item);
    }
  }
}
