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
  [ODataRoute("odata/CodewareDb/Memberships")]
  public partial class MembershipsController : Controller
  {
    private CodewareDbContext context;

    public MembershipsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Memberships
    [HttpGet]
    public IEnumerable<Membership> Get()
    {
      var items = this.context.Memberships.AsQueryable<Membership>();

      this.OnMembershipsRead(ref items);

      return items;
    }

    partial void OnMembershipsRead(ref IQueryable<Membership> items);

    [HttpGet("{UserId}")]
    public IActionResult GetMembership(Guid key)
    {
        var item = this.context.Memberships.Where(i=>i.UserId == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnMembershipDeleted(Membership item);

    [HttpDelete("{UserId}")]
    public IActionResult DeleteMembership(Guid key)
    {
        var item = this.context.Memberships
            .Where(i => i.UserId == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnMembershipDeleted(item);
        this.context.Memberships.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnMembershipUpdated(Membership item);

    [HttpPut("{UserId}")]
    public IActionResult PutMembership(Guid key, [FromBody]Membership newItem)
    {
        if (newItem == null || newItem.UserId != key)
        {
            return BadRequest();
        }

        this.OnMembershipUpdated(newItem);
        this.context.Memberships.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{UserId}")]
    public IActionResult PatchMembership(Guid key, [FromBody]JObject patch)
    {
        var item = this.context.Memberships.Where(i=>i.UserId == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnMembershipUpdated(item);
        this.context.Memberships.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnMembershipCreated(Membership item);

    [HttpPost]
    public IActionResult Post([FromBody] Membership item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnMembershipCreated(item);
        this.context.Memberships.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Memberships/{item.UserId}", item);
    }
  }
}
