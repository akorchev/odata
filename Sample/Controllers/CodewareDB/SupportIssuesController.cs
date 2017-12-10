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
  [ODataRoute("odata/CodewareDb/SupportIssues")]
  public partial class SupportIssuesController : Controller
  {
    private CodewareDbContext context;

    public SupportIssuesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/SupportIssues
    [HttpGet]
    public IEnumerable<SupportIssue> Get()
    {
      var items = this.context.SupportIssues.AsQueryable<SupportIssue>();

      this.OnSupportIssuesRead(ref items);

      return items;
    }

    partial void OnSupportIssuesRead(ref IQueryable<SupportIssue> items);

    [HttpGet("{SupportID}")]
    public IActionResult GetSupportIssue(int key)
    {
        var item = this.context.SupportIssues.Where(i=>i.SupportID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnSupportIssueDeleted(SupportIssue item);

    [HttpDelete("{SupportID}")]
    public IActionResult DeleteSupportIssue(int key)
    {
        var item = this.context.SupportIssues
            .Where(i => i.SupportID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnSupportIssueDeleted(item);
        this.context.SupportIssues.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportIssueUpdated(SupportIssue item);

    [HttpPut("{SupportID}")]
    public IActionResult PutSupportIssue(int key, [FromBody]SupportIssue newItem)
    {
        if (newItem == null || newItem.SupportID != key)
        {
            return BadRequest();
        }

        this.OnSupportIssueUpdated(newItem);
        this.context.SupportIssues.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{SupportID}")]
    public IActionResult PatchSupportIssue(int key, [FromBody]JObject patch)
    {
        var item = this.context.SupportIssues.Where(i=>i.SupportID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnSupportIssueUpdated(item);
        this.context.SupportIssues.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnSupportIssueCreated(SupportIssue item);

    [HttpPost]
    public IActionResult Post([FromBody] SupportIssue item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnSupportIssueCreated(item);
        this.context.SupportIssues.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/SupportIssues/{item.SupportID}", item);
    }
  }
}
