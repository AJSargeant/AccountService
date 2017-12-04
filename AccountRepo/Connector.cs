using System;
using System.Collections.Generic;
using System.Text;
using AccountModel;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AccountRepo
{
    public static class Connector
    {

        private static AccountContext db;
        private static IConfiguration _config;


        public static void Connect(IConfiguration ic, DbContext ac)
        {
            _config = ic;
            db = (AccountContext)ac;
        }


        #region Manager
        public static IEnumerable<Manager> Managers()
        {
            List<AccountModel.Manager> lis = db.Managers.Where(m => m.Active).ToList();
            List<Manager> mlist = new List<Manager>();

            foreach (AccountModel.Manager m in lis)
            {
                mlist.Add(new Manager(m));
            }

            return mlist.AsEnumerable();
        }

        public static Manager Manager(int id)
        {
            AccountModel.Manager man = db.Managers.SingleOrDefault(m => m.UserID == id && m.Active);
            if (man != null)
                return new Manager(man);
            return null;
        }

        public static void AddManager(Manager m)
        {
            db.Managers.Add(m.ToDbType());
        }

        public static void EditManager(Manager m)
        {

        }

        public static void DeleteManager(int id)
        {

        }
        #endregion

        #region Staff
        public static IEnumerable<Staff> Staff()
        {
            List<AccountModel.Staff> lis = db.Staff.Where(s => s.Active).ToList();
            List<Staff> slist = new List<Staff>();

            foreach (AccountModel.Staff s in lis)
            {
                slist.Add(new Staff(s));
            }

            return slist.AsEnumerable();
        }

        public static Staff Staff(int id)
        {
            AccountModel.Staff sta = db.Staff.SingleOrDefault(s => s.UserID == id && s.Active);
            if (sta != null)
                return new Staff(sta);
            return null;
        }

        public static void AddStaff(Staff s)
        {
            db.Staff.Add(s.ToDbType());
            db.SaveChanges();
        }

        public static void EditStaff(Staff s)
        {

        }

        public static void DeleteStaff(int id)
        {

        }
        #endregion

        #region User
        public static IEnumerable<User> Users()
        {
            List<AccountModel.User> lis = db.Users.Where(u => u.Active).ToList();
            List<User> ulist = new List<User>();

            foreach (AccountModel.User u in lis)
            {
                ulist.Add(new User(u));
            }

            return ulist.AsEnumerable();
        }

        public static User User(int id)
        {
            AccountModel.User use = db.Users.SingleOrDefault(u => u.UserID == id && u.Active);
            if (use != null)
                return new User(use);
            return null;
        }

        public static void AddUser(User u)
        {
            db.Users.Add(u.ToDbType());
            db.SaveChanges();
        }

        public static void EditUser(User m)
        {

        }

        public static void DeleteUser(int id)
        {

        }
        #endregion
    }
}
