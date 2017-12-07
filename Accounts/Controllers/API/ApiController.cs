using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AccountModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounts.Controllers.API
{
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        AccountContext db;

        public ApiController(AccountContext context)
        { db = context; }

        // GET: api/values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            try
            {
                return db.Users.Where(u => u.Active);
            }
            catch { return null; }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User Get(string id)
        {
            try
            {
                User user = db.Users.FirstOrDefault(u => u.UserID == id);
                if (user != null)
                    if (user.Active)
                        return user;
                return null;
            }
            catch { return null; }
        }

        // POST api/values
        [HttpPost]
        [Route("SaveUser")]
        public IActionResult SaveAuthUser([FromBody]Models.AuthUser au)
        {
            if (au == null)
                return new StatusCodeResult(400);
            try
            {
                User u = new User
                {
                    UserID = au.ID,
                    Email = au.Email,
                    Active = true,
                    Authorised = false,
                    Role = "User"
                };
                db.Users.Add(u);
                db.SaveChanges();
                return new StatusCodeResult(200);
            }
            catch { return new StatusCodeResult(500); }
        }

        // POST api/values
        [HttpPost]
        [Route("SaveUser2")]
        public IActionResult SaveUser([FromBody]User u)
        {
            if (u == null)
                return new StatusCodeResult(400);
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return new StatusCodeResult(200);
            }
            catch { return new StatusCodeResult(500); }
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Route("RemoveUser")]
        public IActionResult Delete(string id)
        {
            try
            {
                User user = db.Users.FirstOrDefault(u => u.UserID == id);
                if (user != null)
                    if (user.Active)
                    {
                        user.Active = false;
                        user.Authorised = false;
                        user.Role = "User";

                        db.SaveChanges();
                        return new StatusCodeResult(200);
                    }
                return new StatusCodeResult(400);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
