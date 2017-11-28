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
        AccountContext db;

        public StaffController(AccountContext context)
        { db = context; }

        [HttpGet]
        public IEnumerable<Staff> Get()
        {
            return db.Staff;
        }

        [HttpGet("{id}")]
        public Staff Get(int id)
        {
            return db.Staff.FirstOrDefault(s => s.UserID == id);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]Staff s)
        {
            db.Staff.Add(s);
            db.SaveChanges();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Staff staff = db.Staff.FirstOrDefault(s => s.UserID == id);
            if(staff != null)
                if (staff.Active)
                {
                    staff.Active = false;
                    db.SaveChanges();
                }
        }
    }
}