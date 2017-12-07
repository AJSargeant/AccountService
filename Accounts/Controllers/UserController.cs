using System;
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

        public async Task<ActionResult> ChangeAuthorisation(string id)
        {
            if (id == null)
                return new StatusCodeResult(400);
            try
            {
                if (User.IsInRole("Staff") || User.IsInRole("Admin"))
                {
                    User u = new User();
                    try
                    {
                         u = db.Users.Single(use => use.UserID == id);
                    }
                    catch
                    {
                        return new StatusCodeResult(404);
                    }

                    u.Authorised = !u.Authorised;
                    u.Role = "Customer";
                    db.SaveChanges();
                    try
                    {
                        await PostToAuth(id, u.Authorised);
                    }
                    catch { }

                    return RedirectToAction(nameof(Index));
                }
                return new StatusCodeResult(403);
            }
            catch { return new StatusCodeResult(500); }
        }

        private async Task PostToAuth(string id, bool auth)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:58253/Account/RecieveAuthStatus")
            };
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var values = new Dictionary<string, string>();
            values.Add("UserID", id);
            values.Add("AuthorisationStatus", auth.ToString());
            await client.PostAsync(client.BaseAddress.ToString(),new FormUrlEncodedContent(values));
        }

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                if(User.IsInRole("Staff")||User.IsInRole("Admin"))
                    return View(db.Users.Where(u => u.Active && (u.Role == "User" ||u.Role == "Customer")));
                return new StatusCodeResult(403);
            }
            catch { return new StatusCodeResult(500); }
        }

        // GET: User/Profile/5
        [HttpGet]
        public ActionResult Profile(string id)
        {
            if (id == null)
                return new StatusCodeResult(400);
            try
            {
                User u = db.Users.SingleOrDefault(user => user.UserID == id && user.Active);
                if (u == null)
                    return new StatusCodeResult(404);

                string UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (UserID == id || User.IsInRole("Staff") || User.IsInRole("Admin"))
                    return View(u);

                return new StatusCodeResult(403);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        // GET: User/Edit/5
        [HttpGet]
        public ActionResult EditProfile(string id)
        {
            if (id == null)
                return new StatusCodeResult(400);
            try
            {
                User u = db.Users.Single(user => user.UserID == id && user.Active);
                if (u == null)
                    return new StatusCodeResult(404);

                string UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (UserID == id)
                    return View(u);

                return new StatusCodeResult(403);
            }
            catch { return new StatusCodeResult(500); }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfilePost(User u)
        {
            if (u == null)
                return new StatusCodeResult(400);
            try
            {
                string UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (UserID == u.UserID)
                {
                    User use = db.Users.Single(user => user.UserID == u.UserID);

                    use.Name = u.Name;
                    use.Email = u.Email;
                    db.SaveChanges();

                    return RedirectToAction(nameof(Profile), new { id = UserID });
                }
                return new StatusCodeResult(403);
            }
            catch { return new StatusCodeResult(500); }
        }

        public ActionResult ViewMessages(string id)
        {
            string UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(UserID == id || User.IsInRole("Staff") || User.IsInRole("Admin"))
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