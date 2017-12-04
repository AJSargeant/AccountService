﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AccountModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Http;

namespace Accounts.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        AccountContext db;

        public UserController(AccountContext context)
        {
            db = context;
        }

        public async Task<ActionResult> ChangeAuthorisation(int id)
        {
            if (User.IsInRole("Staff") || User.IsInRole("Administrator"))
            {
                User u = db.Users.Single(use => use.UserID == id);

                u.Authorised = !u.Authorised;
                db.SaveChanges();
                try
                {
                    await PostToAuth(id, u.Authorised);
                }
                catch{}

                return RedirectToAction(nameof(Index));
            }
            return new StatusCodeResult(403);
        }

        private async Task PostToAuth(int id, bool auth)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:51520")
            };

            var values = new Dictionary<string, string>();
            values.Add("UserID", id.ToString());
            values.Add("Authorisation Status", auth.ToString());
            await client.PostAsync(client.BaseAddress.ToString(),new FormUrlEncodedContent(values));
        }

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            if(User.IsInRole("Staff")||User.IsInRole("Administrator"))
                return View(db.Users.Where(u => u.Active));
            return new StatusCodeResult(403);
        }

        // GET: User/Profile/5
        [HttpGet]
        public ActionResult Profile(int id)
        {
            User u = db.Users.SingleOrDefault(user => user.UserID == id && user.Active);
            if (u == null)
                return new StatusCodeResult(404);

            int UserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (UserID == id || User.IsInRole("Staff") || User.IsInRole("Administrator"))
                return View(u);
            
            return new StatusCodeResult(403);
        }

        // GET: User/Edit/5
        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            User u = db.Users.Single(user => user.UserID == id && user.Active);
            if (u == null)
                return new StatusCodeResult(404);

            int UserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (UserID == id)
                return View(u);

            return new StatusCodeResult(403);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfilePost([FromBody]User u)
        {
            int UserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (UserID == u.ID)
            {
                try
                {
                    User use = db.Users.Single(user => user.UserID == u.ID);

                    use.Name = u.Name;
                    use.Email = u.Email;
                    db.SaveChanges();

                    return RedirectToAction(nameof(Profile),new { id = UserID });
                }
                catch
                {
                    return View(u);
                }
            }
            return new StatusCodeResult(403);
        }

        public ActionResult ViewMessages(int id)
        {
            int UserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(UserID == id || User.IsInRole("Staff") || User.IsInRole("Administrator"))
            {
                try
                {
                    return Redirect("Http://localhost:50143/Message/MyMessages/" + id.ToString());
                }
                catch
                {
                    return new StatusCodeResult(404);
                }
            }
            return new StatusCodeResult(403);
        }
    }
}