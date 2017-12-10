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
  [ODataRoute("odata/CodewareDb/RolePermissionSets")]
  public partial class RolePermissionSetsController : Controller
  {
    private CodewareDbContext context;

    public RolePermissionSetsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/RolePermissionSets
    [HttpGet]
    public IEnumerable<RolePermissionSet> Get()
    {
      var items = this.context.RolePermissionSets.AsQueryable<RolePermissionSet>();

      this.OnRolePermissionSetsRead(ref items);

      return items;
    }

    partial void OnRolePermissionSetsRead(ref IQueryable<RolePermissionSet> items);

    [HttpGet("{PermissionId}")]
    public IActionResult GetRolePermissionSet(string key)
    {
        var item = this.context.RolePermissionSets.Where(i=>i.PermissionId == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnRolePermissionSetDeleted(RolePermissionSet item);

    [HttpDelete("{PermissionId}")]
    public IActionResult DeleteRolePermissionSet(string key)
    {
        var item = this.context.RolePermissionSets
            .Where(i => i.PermissionId == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnRolePermissionSetDeleted(item);
        this.context.RolePermissionSets.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRolePermissionSetUpdated(RolePermissionSet item);

    [HttpPut("{PermissionId}")]
    public IActionResult PutRolePermissionSet(string key, [FromBody]RolePermissionSet newItem)
    {
        if (newItem == null || newItem.PermissionId != key)
        {
            return BadRequest();
        }

        this.OnRolePermissionSetUpdated(newItem);
        this.context.RolePermissionSets.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{PermissionId}")]
    public IActionResult PatchRolePermissionSet(string key, [FromBody]JObject patch)
    {
        var item = this.context.RolePermissionSets.Where(i=>i.PermissionId == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnRolePermissionSetUpdated(item);
        this.context.RolePermissionSets.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRolePermissionSetCreated(RolePermissionSet item);

    [HttpPost]
    public IActionResult Post([FromBody] RolePermissionSet item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnRolePermissionSetCreated(item);
        this.context.RolePermissionSets.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/RolePermissionSets/{item.PermissionId}", item);
    }
  }
}
