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
  [ODataRoute("odata/CodewareDb/ProjectMembers")]
  public partial class ProjectMembersController : Controller
  {
    private CodewareDbContext context;

    public ProjectMembersController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/ProjectMembers
    [HttpGet]
    public IEnumerable<ProjectMember> Get()
    {
      var items = this.context.ProjectMembers.AsQueryable<ProjectMember>();

      this.OnProjectMembersRead(ref items);

      return items;
    }

    partial void OnProjectMembersRead(ref IQueryable<ProjectMember> items);

    [HttpGet("{ProjectID}")]
    public IActionResult GetProjectMember(int key)
    {
        var item = this.context.ProjectMembers.Where(i=>i.ProjectID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnProjectMemberDeleted(ProjectMember item);

    [HttpDelete("{ProjectID}")]
    public IActionResult DeleteProjectMember(int key)
    {
        var item = this.context.ProjectMembers
            .Where(i => i.ProjectID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnProjectMemberDeleted(item);
        this.context.ProjectMembers.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectMemberUpdated(ProjectMember item);

    [HttpPut("{ProjectID}")]
    public IActionResult PutProjectMember(int key, [FromBody]ProjectMember newItem)
    {
        if (newItem == null || newItem.ProjectID != key)
        {
            return BadRequest();
        }

        this.OnProjectMemberUpdated(newItem);
        this.context.ProjectMembers.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ProjectID}")]
    public IActionResult PatchProjectMember(int key, [FromBody]JObject patch)
    {
        var item = this.context.ProjectMembers.Where(i=>i.ProjectID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnProjectMemberUpdated(item);
        this.context.ProjectMembers.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectMemberCreated(ProjectMember item);

    [HttpPost]
    public IActionResult Post([FromBody] ProjectMember item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProjectMemberCreated(item);
        this.context.ProjectMembers.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/ProjectMembers/{item.ProjectID}", item);
    }
  }
}
