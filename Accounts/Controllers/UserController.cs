using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AccountModel;

namespace Accounts.Controllers
{
    public class UserController : Controller
    {
        AccountContext db;

        public UserController(AccountContext context)
        {
            db = context;
        }

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Users.Where(u => u.Active));
        }

        // GET: User/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(db.Users.Single(u => u.UserID == id && u.Active));
        }

        // GET: User/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}