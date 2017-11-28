using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AccountModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountManagement
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        AccountContext db;

        public UserController(AccountContext context)
        { db = context; }

        // GET: api/values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return db.Users.Where(u => u.Active);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
                if (user.Active)
                    return user;
            return null;
        }

        // POST api/values
        [HttpPost]
        [Route("SaveUser")]
        public void Post([FromBody]User u)
        {
            u.Active = true;
            u.Authorised = false;
            db.Users.Add(u);
            db.SaveChanges();
        }
        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Route("RemoveUser")]
        public void Delete(int id)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
                if (user.Active)
                {
                    user.Active = false;
                    user.Authorised = false;
                    db.SaveChanges();
                }
        }
    }
}
