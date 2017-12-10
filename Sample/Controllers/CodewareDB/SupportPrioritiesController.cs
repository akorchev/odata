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
  [ODataRoute("odata/CodewareDb/SupportPriorities")]
  public partial class SupportPrioritiesController : Controller
  {
    private CodewareDbContext context;

    public SupportPrioritiesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/SupportPriorities
    [HttpGet]
    public IEnumerable<SupportPriority> Get()
    {
      var items = this.context.SupportPriorities.AsQueryable<SupportPriority>();

      this.OnSupportPrioritiesRead(ref items);

      return items;
    }

    partial void OnSupportPrioritiesRead(ref IQueryable<SupportPriority> items);

    [HttpGet("{Priority}")]
    public IActionResult GetSupportPriority(string key)
    {
        var item = this.context.SupportPriorities.Where(i=>i.Priority == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnSupportPriorityDeleted(SupportPriority item);

    [HttpDelete("{Priority}")]
    public IActionResult DeleteSupportPriority(string key)
    {
        var item = this.context.SupportPriorities
            .Where(i => i.Priority == key)
            .Include(i => i.SupportIssues)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnSupportPriorityDeleted(item);
        this.context.SupportPriorities.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportPriorityUpdated(SupportPriority item);

    [HttpPut("{Priority}")]
    public IActionResult PutSupportPriority(string key, [FromBody]SupportPriority newItem)
    {
        if (newItem == null || newItem.Priority != key)
        {
            return BadRequest();
        }

        this.OnSupportPriorityUpdated(newItem);
        this.context.SupportPriorities.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Priority}")]
    public IActionResult PatchSupportPriority(string key, [FromBody]JObject patch)
    {
        var item = this.context.SupportPriorities.Where(i=>i.Priority == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnSupportPriorityUpdated(item);
        this.context.SupportPriorities.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportPriorityCreated(SupportPriority item);

    [HttpPost]
    public IActionResult Post([FromBody] SupportPriority item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnSupportPriorityCreated(item);
        this.context.SupportPriorities.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/SupportPriorities/{item.Priority}", item);
    }
  }
}
