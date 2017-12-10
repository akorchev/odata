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
  [ODataRoute("odata/CodewareDb/CustomerContacts")]
  public partial class CustomerContactsController : Controller
  {
    private CodewareDbContext context;

    public CustomerContactsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/CustomerContacts
    [HttpGet]
    public IEnumerable<CustomerContact> Get()
    {
      var items = this.context.CustomerContacts.AsQueryable<CustomerContact>();

      this.OnCustomerContactsRead(ref items);

      return items;
    }

    partial void OnCustomerContactsRead(ref IQueryable<CustomerContact> items);

    [HttpGet("{ContactID}")]
    public IActionResult GetCustomerContact(int key)
    {
        var item = this.context.CustomerContacts.Where(i=>i.ContactID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnCustomerContactDeleted(CustomerContact item);

    [HttpDelete("{ContactID}")]
    public IActionResult DeleteCustomerContact(int key)
    {
        var item = this.context.CustomerContacts
            .Where(i => i.ContactID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCustomerContactDeleted(item);
        this.context.CustomerContacts.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerContactUpdated(CustomerContact item);

    [HttpPut("{ContactID}")]
    public IActionResult PutCustomerContact(int key, [FromBody]CustomerContact newItem)
    {
        if (newItem == null || newItem.ContactID != key)
        {
            return BadRequest();
        }

        this.OnCustomerContactUpdated(newItem);
        this.context.CustomerContacts.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ContactID}")]
    public IActionResult PatchCustomerContact(int key, [FromBody]JObject patch)
    {
        var item = this.context.CustomerContacts.Where(i=>i.ContactID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnCustomerContactUpdated(item);
        this.context.CustomerContacts.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerContactCreated(CustomerContact item);

    [HttpPost]
    public IActionResult Post([FromBody] CustomerContact item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCustomerContactCreated(item);
        this.context.CustomerContacts.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/CustomerContacts/{item.ContactID}", item);
    }
  }
}
