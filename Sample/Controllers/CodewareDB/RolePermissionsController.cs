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
  [ODataRoute("odata/CodewareDb/RolePermissions")]
  public partial class RolePermissionsController : Controller
  {
    private CodewareDbContext context;

    public RolePermissionsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/RolePermissions
    [HttpGet]
    public IEnumerable<RolePermission> Get()
    {
      var items = this.context.RolePermissions.AsQueryable<RolePermission>();

      this.OnRolePermissionsRead(ref items);

      return items;
    }

    partial void OnRolePermissionsRead(ref IQueryable<RolePermission> items);

    [HttpGet("{PermissionId}")]
    public IActionResult GetRolePermission(string key)
    {
        var item = this.context.RolePermissions.Where(i=>i.PermissionId == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnRolePermissionDeleted(RolePermission item);

    [HttpDelete("{PermissionId}")]
    public IActionResult DeleteRolePermission(string key)
    {
        var item = this.context.RolePermissions
            .Where(i => i.PermissionId == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnRolePermissionDeleted(item);
        this.context.RolePermissions.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRolePermissionUpdated(RolePermission item);

    [HttpPut("{PermissionId}")]
    public IActionResult PutRolePermission(string key, [FromBody]RolePermission newItem)
    {
        if (newItem == null || newItem.PermissionId != key)
        {
            return BadRequest();
        }

        this.OnRolePermissionUpdated(newItem);
        this.context.RolePermissions.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{PermissionId}")]
    public IActionResult PatchRolePermission(string key, [FromBody]JObject patch)
    {
        var item = this.context.RolePermissions.Where(i=>i.PermissionId == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnRolePermissionUpdated(item);
        this.context.RolePermissions.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRolePermissionCreated(RolePermission item);

    [HttpPost]
    public IActionResult Post([FromBody] RolePermission item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnRolePermissionCreated(item);
        this.context.RolePermissions.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/RolePermissions/{item.PermissionId}", item);
    }
  }
}
