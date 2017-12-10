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
  [ODataRoute("odata/CodewareDb/Projects")]
  public partial class ProjectsController : Controller
  {
    private CodewareDbContext context;

    public ProjectsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Projects
    [HttpGet]
    public IEnumerable<Project> Get()
    {
      var items = this.context.Projects.AsQueryable<Project>();

      this.OnProjectsRead(ref items);

      return items;
    }

    partial void OnProjectsRead(ref IQueryable<Project> items);

    [HttpGet("{ProjectID}")]
    public IActionResult GetProject(int key)
    {
        var item = this.context.Projects.Where(i=>i.ProjectID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnProjectDeleted(Project item);

    [HttpDelete("{ProjectID}")]
    public IActionResult DeleteProject(int key)
    {
        var item = this.context.Projects
            .Where(i => i.ProjectID == key)
            .Include(i => i.Details)
            .Include(i => i.Categories)
            .Include(i => i.ProjectMembers)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnProjectDeleted(item);
        this.context.Projects.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectUpdated(Project item);

    [HttpPut("{ProjectID}")]
    public IActionResult PutProject(int key, [FromBody]Project newItem)
    {
        if (newItem == null || newItem.ProjectID != key)
        {
            return BadRequest();
        }

        this.OnProjectUpdated(newItem);
        this.context.Projects.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ProjectID}")]
    public IActionResult PatchProject(int key, [FromBody]JObject patch)
    {
        var item = this.context.Projects.Where(i=>i.ProjectID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnProjectUpdated(item);
        this.context.Projects.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectCreated(Project item);

    [HttpPost]
    public IActionResult Post([FromBody] Project item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProjectCreated(item);
        this.context.Projects.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Projects/{item.ProjectID}", item);
    }
  }
}
