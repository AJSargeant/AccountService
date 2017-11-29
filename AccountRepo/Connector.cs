using System;
using System.Collections.Generic;
using System.Text;
using AccountModel;
using System.Linq;

namespace AccountRepo
{
    public static class Connector
    {
        private static AccountContext db = new AccountContext();

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
    }
}
