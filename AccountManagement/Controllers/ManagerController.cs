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
            return db.Managers.Where(m => m.Active);
        }

        // GET: api/Manager/5
        [HttpGet("{id}", Name = "Get")]
        public Manager Get(int id)
        {
            Manager man = db.Managers.Where(m => m.UserID == id).FirstOrDefault();
            if (man != null)
                if(man.Active)
                    return man;
            return null;
        }


        // POST: api/Manager
        [HttpPost]
        [Route("SaveManager")]
        public void Post([FromBody]Manager m)
        {
            m.Active = true;
            db.Managers.Add(m);
            db.SaveChanges();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Route("RemoveManager")]
        public void Delete(int id)
        {
            Manager man = db.Managers.Where(m => m.UserID == id).First();
            man.Active = false;
            db.SaveChanges();
        }
    }
}
