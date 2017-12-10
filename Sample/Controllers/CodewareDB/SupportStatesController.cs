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
  [ODataRoute("odata/CodewareDb/SupportStates")]
  public partial class SupportStatesController : Controller
  {
    private CodewareDbContext context;

    public SupportStatesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/SupportStates
    [HttpGet]
    public IEnumerable<SupportState> Get()
    {
      var items = this.context.SupportStates.AsQueryable<SupportState>();

      this.OnSupportStatesRead(ref items);

      return items;
    }

    partial void OnSupportStatesRead(ref IQueryable<SupportState> items);

    [HttpGet("{State}")]
    public IActionResult GetSupportState(string key)
    {
        var item = this.context.SupportStates.Where(i=>i.State == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnSupportStateDeleted(SupportState item);

    [HttpDelete("{State}")]
    public IActionResult DeleteSupportState(string key)
    {
        var item = this.context.SupportStates
            .Where(i => i.State == key)
            .Include(i => i.SupportIssues)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnSupportStateDeleted(item);
        this.context.SupportStates.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportStateUpdated(SupportState item);

    [HttpPut("{State}")]
    public IActionResult PutSupportState(string key, [FromBody]SupportState newItem)
    {
        if (newItem == null || newItem.State != key)
        {
            return BadRequest();
        }

        this.OnSupportStateUpdated(newItem);
        this.context.SupportStates.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{State}")]
    public IActionResult PatchSupportState(string key, [FromBody]JObject patch)
    {
        var item = this.context.SupportStates.Where(i=>i.State == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnSupportStateUpdated(item);
        this.context.SupportStates.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportStateCreated(SupportState item);

    [HttpPost]
    public IActionResult Post([FromBody] SupportState item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnSupportStateCreated(item);
        this.context.SupportStates.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/SupportStates/{item.State}", item);
    }
  }
}
