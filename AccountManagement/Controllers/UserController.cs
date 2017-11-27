﻿using System;
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
        AccountContext db = new AccountContext();

        // GET: api/values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            User u = new User()
            {
                Name = "user",
                ID = 1,
                Active = true
            };

            db.Users.Add(u);
            db.SaveChanges();

            return db.Users;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return db.Users.Where(u => u.ID == id).FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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