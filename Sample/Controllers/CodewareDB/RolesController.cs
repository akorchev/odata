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
  [ODataRoute("odata/CodewareDb/Roles")]
  public partial class RolesController : Controller
  {
    private CodewareDbContext context;

    public RolesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Roles
    [HttpGet]
    public IEnumerable<Role> Get()
    {
      var items = this.context.Roles.AsQueryable<Role>();

      this.OnRolesRead(ref items);

      return items;
    }

    partial void OnRolesRead(ref IQueryable<Role> items);

    [HttpGet("{RoleID}")]
    public IActionResult GetRole(int key)
    {
        var item = this.context.Roles.Where(i=>i.RoleID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnRoleDeleted(Role item);

    [HttpDelete("{RoleID}")]
    public IActionResult DeleteRole(int key)
    {
        var item = this.context.Roles
            .Where(i => i.RoleID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnRoleDeleted(item);
        this.context.Roles.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRoleUpdated(Role item);

    [HttpPut("{RoleID}")]
    public IActionResult PutRole(int key, [FromBody]Role newItem)
    {
        if (newItem == null || newItem.RoleID != key)
        {
            return BadRequest();
        }

        this.OnRoleUpdated(newItem);
        this.context.Roles.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{RoleID}")]
    public IActionResult PatchRole(int key, [FromBody]JObject patch)
    {
        var item = this.context.Roles.Where(i=>i.RoleID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnRoleUpdated(item);
        this.context.Roles.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRoleCreated(Role item);

    [HttpPost]
    public IActionResult Post([FromBody] Role item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnRoleCreated(item);
        this.context.Roles.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Roles/{item.RoleID}", item);
    }
  }
}
