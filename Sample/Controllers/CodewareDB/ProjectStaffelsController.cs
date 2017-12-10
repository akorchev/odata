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
  [ODataRoute("odata/CodewareDb/ProjectStaffels")]
  public partial class ProjectStaffelsController : Controller
  {
    private CodewareDbContext context;

    public ProjectStaffelsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/ProjectStaffels
    [HttpGet]
    public IEnumerable<ProjectStaffel> Get()
    {
      var items = this.context.ProjectStaffels.AsQueryable<ProjectStaffel>();

      this.OnProjectStaffelsRead(ref items);

      return items;
    }

    partial void OnProjectStaffelsRead(ref IQueryable<ProjectStaffel> items);

    [HttpGet("{ProjectStaffelID}")]
    public IActionResult GetProjectStaffel(int key)
    {
        var item = this.context.ProjectStaffels.Where(i=>i.ProjectStaffelID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnProjectStaffelDeleted(ProjectStaffel item);

    [HttpDelete("{ProjectStaffelID}")]
    public IActionResult DeleteProjectStaffel(int key)
    {
        var item = this.context.ProjectStaffels
            .Where(i => i.ProjectStaffelID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnProjectStaffelDeleted(item);
        this.context.ProjectStaffels.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectStaffelUpdated(ProjectStaffel item);

    [HttpPut("{ProjectStaffelID}")]
    public IActionResult PutProjectStaffel(int key, [FromBody]ProjectStaffel newItem)
    {
        if (newItem == null || newItem.ProjectStaffelID != key)
        {
            return BadRequest();
        }

        this.OnProjectStaffelUpdated(newItem);
        this.context.ProjectStaffels.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ProjectStaffelID}")]
    public IActionResult PatchProjectStaffel(int key, [FromBody]JObject patch)
    {
        var item = this.context.ProjectStaffels.Where(i=>i.ProjectStaffelID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnProjectStaffelUpdated(item);
        this.context.ProjectStaffels.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectStaffelCreated(ProjectStaffel item);

    [HttpPost]
    public IActionResult Post([FromBody] ProjectStaffel item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProjectStaffelCreated(item);
        this.context.ProjectStaffels.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/ProjectStaffels/{item.ProjectStaffelID}", item);
    }
  }
}
