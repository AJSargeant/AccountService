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
            Staff s = new Staff()
            {
                Name = "Staff",
                ID = 1,
                Active = true
            };

            db.Staff.Add(s);
            db.SaveChanges();

            return db.Staff;
        }

        [HttpGet("{id}")]
        public Staff Get(int id)
        {
            return db.Staff.Where(s => s.ID == id).FirstOrDefault();
        }

    }
}