using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountModel;

namespace AccountManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Staff")]
    public class StaffController : Controller
    {
        AccountContext db = new AccountContext();

        [HttpGet]
        public IEnumerable<Staff> Get()
        {
            return db.Staff;
        }

        [HttpGet("{id}")]
        public Staff Get(int id)
        {
            return db.Staff.Where(s => s.UserID == id).FirstOrDefault();
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]Staff s)
        {
            db.Staff.Add(s);
            db.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}