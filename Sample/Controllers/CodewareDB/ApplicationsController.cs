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
  [ODataRoute("odata/CodewareDb/Applications")]
  public partial class ApplicationsController : Controller
  {
    private CodewareDbContext context;

    public ApplicationsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Applications
    [HttpGet]
    public IEnumerable<Application> Get()
    {
      var items = this.context.Applications.AsQueryable<Application>();

      this.OnApplicationsRead(ref items);

      return items;
    }

    partial void OnApplicationsRead(ref IQueryable<Application> items);

    [HttpGet("{ApplicationId}")]
    public IActionResult GetApplication(Guid key)
    {
        var item = this.context.Applications.Where(i=>i.ApplicationId == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnApplicationDeleted(Application item);

    [HttpDelete("{ApplicationId}")]
    public IActionResult DeleteApplication(Guid key)
    {
        var item = this.context.Applications
            .Where(i => i.ApplicationId == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnApplicationDeleted(item);
        this.context.Applications.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnApplicationUpdated(Application item);

    [HttpPut("{ApplicationId}")]
    public IActionResult PutApplication(Guid key, [FromBody]Application newItem)
    {
        if (newItem == null || newItem.ApplicationId != key)
        {
            return BadRequest();
        }

        this.OnApplicationUpdated(newItem);
        this.context.Applications.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ApplicationId}")]
    public IActionResult PatchApplication(Guid key, [FromBody]JObject patch)
    {
        var item = this.context.Applications.Where(i=>i.ApplicationId == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnApplicationUpdated(item);
        this.context.Applications.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnApplicationCreated(Application item);

    [HttpPost]
    public IActionResult Post([FromBody] Application item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnApplicationCreated(item);
        this.context.Applications.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Applications/{item.ApplicationId}", item);
    }
  }
}
