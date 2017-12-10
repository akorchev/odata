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
  [ODataRoute("odata/CodewareDb/Mails")]
  public partial class MailsController : Controller
  {
    private CodewareDbContext context;

    public MailsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Mails
    [HttpGet]
    public IEnumerable<Mail> Get()
    {
      var items = this.context.Mails.AsQueryable<Mail>();

      this.OnMailsRead(ref items);

      return items;
    }

    partial void OnMailsRead(ref IQueryable<Mail> items);

    [HttpGet("{MailID}")]
    public IActionResult GetMail(int key)
    {
        var item = this.context.Mails.Where(i=>i.MailID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnMailDeleted(Mail item);

    [HttpDelete("{MailID}")]
    public IActionResult DeleteMail(int key)
    {
        var item = this.context.Mails
            .Where(i => i.MailID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnMailDeleted(item);
        this.context.Mails.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnMailUpdated(Mail item);

    [HttpPut("{MailID}")]
    public IActionResult PutMail(int key, [FromBody]Mail newItem)
    {
        if (newItem == null || newItem.MailID != key)
        {
            return BadRequest();
        }

        this.OnMailUpdated(newItem);
        this.context.Mails.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{MailID}")]
    public IActionResult PatchMail(int key, [FromBody]JObject patch)
    {
        var item = this.context.Mails.Where(i=>i.MailID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnMailUpdated(item);
        this.context.Mails.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnMailCreated(Mail item);

    [HttpPost]
    public IActionResult Post([FromBody] Mail item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnMailCreated(item);
        this.context.Mails.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Mails/{item.MailID}", item);
    }
  }
}
