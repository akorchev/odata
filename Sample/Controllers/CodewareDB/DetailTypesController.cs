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
  [ODataRoute("odata/CodewareDb/DetailTypes")]
  public partial class DetailTypesController : Controller
  {
    private CodewareDbContext context;

    public DetailTypesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/DetailTypes
    [HttpGet]
    public IEnumerable<DetailType> Get()
    {
      var items = this.context.DetailTypes.AsQueryable<DetailType>();

      this.OnDetailTypesRead(ref items);

      return items;
    }

    partial void OnDetailTypesRead(ref IQueryable<DetailType> items);

    [HttpGet("{Type}")]
    public IActionResult GetDetailType(string key)
    {
        var item = this.context.DetailTypes.Where(i=>i.Type == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnDetailTypeDeleted(DetailType item);

    [HttpDelete("{Type}")]
    public IActionResult DeleteDetailType(string key)
    {
        var item = this.context.DetailTypes
            .Where(i => i.Type == key)
            .Include(i => i.Details)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnDetailTypeDeleted(item);
        this.context.DetailTypes.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDetailTypeUpdated(DetailType item);

    [HttpPut("{Type}")]
    public IActionResult PutDetailType(string key, [FromBody]DetailType newItem)
    {
        if (newItem == null || newItem.Type != key)
        {
            return BadRequest();
        }

        this.OnDetailTypeUpdated(newItem);
        this.context.DetailTypes.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Type}")]
    public IActionResult PatchDetailType(string key, [FromBody]JObject patch)
    {
        var item = this.context.DetailTypes.Where(i=>i.Type == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnDetailTypeUpdated(item);
        this.context.DetailTypes.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDetailTypeCreated(DetailType item);

    [HttpPost]
    public IActionResult Post([FromBody] DetailType item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnDetailTypeCreated(item);
        this.context.DetailTypes.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/DetailTypes/{item.Type}", item);
    }
  }
}
