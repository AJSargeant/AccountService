using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountModel;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Manager")]
    public class ManagerController : Controller
    {
        AccountContext db;
        public ManagerController(AccountContext context)
        { db = context; }

        // GET: api/Manager
        [HttpGet]
        public IEnumerable<Manager> Get()
        {
                Manager m = new Manager()
                {
                    Name = "manager",
                    Active = true
                };

                db.Managers.Add(m);
                db.SaveChanges();
            return db.Managers;
        }

        // GET: api/Manager/5
        [HttpGet("{id}", Name = "Get")]
        public Manager Get(int id)
        {
            return db.Managers.Where(m => m.ID == id).FirstOrDefault();
        }


        // POST: api/Manager
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Manager/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
