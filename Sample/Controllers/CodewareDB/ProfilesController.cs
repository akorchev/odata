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
  [ODataRoute("odata/CodewareDb/Profiles")]
  public partial class ProfilesController : Controller
  {
    private CodewareDbContext context;

    public ProfilesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Profiles
    [HttpGet]
    public IEnumerable<Profile> Get()
    {
      var items = this.context.Profiles.AsQueryable<Profile>();

      this.OnProfilesRead(ref items);

      return items;
    }

    partial void OnProfilesRead(ref IQueryable<Profile> items);

    [HttpGet("{UserId}")]
    public IActionResult GetProfile(Guid key)
    {
        var item = this.context.Profiles.Where(i=>i.UserId == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnProfileDeleted(Profile item);

    [HttpDelete("{UserId}")]
    public IActionResult DeleteProfile(Guid key)
    {
        var item = this.context.Profiles
            .Where(i => i.UserId == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnProfileDeleted(item);
        this.context.Profiles.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProfileUpdated(Profile item);

    [HttpPut("{UserId}")]
    public IActionResult PutProfile(Guid key, [FromBody]Profile newItem)
    {
        if (newItem == null || newItem.UserId != key)
        {
            return BadRequest();
        }

        this.OnProfileUpdated(newItem);
        this.context.Profiles.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{UserId}")]
    public IActionResult PatchProfile(Guid key, [FromBody]JObject patch)
    {
        var item = this.context.Profiles.Where(i=>i.UserId == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnProfileUpdated(item);
        this.context.Profiles.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProfileCreated(Profile item);

    [HttpPost]
    public IActionResult Post([FromBody] Profile item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProfileCreated(item);
        this.context.Profiles.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Profiles/{item.UserId}", item);
    }
  }
}
