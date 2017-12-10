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
  [ODataRoute("odata/CodewareDb/Users")]
  public partial class UsersController : Controller
  {
    private CodewareDbContext context;

    public UsersController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Users
    [HttpGet]
    public IEnumerable<User> Get()
    {
      var items = this.context.Users.AsQueryable<User>();

      this.OnUsersRead(ref items);

      return items;
    }

    partial void OnUsersRead(ref IQueryable<User> items);

    [HttpGet("{UserID}")]
    public IActionResult GetUser(int key)
    {
        var item = this.context.Users.Where(i=>i.UserID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnUserDeleted(User item);

    [HttpDelete("{UserID}")]
    public IActionResult DeleteUser(int key)
    {
        var item = this.context.Users
            .Where(i => i.UserID == key)
            .Include(i => i.Details)
            .Include(i => i.KnowledgeBases)
            .Include(i => i.CustomerKnowledgeBases)
            .Include(i => i.Projects)
            .Include(i => i.ProjectMembers)
            .Include(i => i.SupportIssues)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnUserDeleted(item);
        this.context.Users.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnUserUpdated(User item);

    [HttpPut("{UserID}")]
    public IActionResult PutUser(int key, [FromBody]User newItem)
    {
        if (newItem == null || newItem.UserID != key)
        {
            return BadRequest();
        }

        this.OnUserUpdated(newItem);
        this.context.Users.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{UserID}")]
    public IActionResult PatchUser(int key, [FromBody]JObject patch)
    {
        var item = this.context.Users.Where(i=>i.UserID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnUserUpdated(item);
        this.context.Users.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnUserCreated(User item);

    [HttpPost]
    public IActionResult Post([FromBody] User item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnUserCreated(item);
        this.context.Users.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Users/{item.UserID}", item);
    }
  }
}
