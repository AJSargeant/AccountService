using System;
using System.Collections.Generic;
using System.Text;

namespace AccountRepo
{
    public class Staff
    {
        public Staff(AccountModel.Staff s)
        {
            ID = s.ID;
            UserID = s.UserID;
            Name = s.Name;
            Active = s.Active;
        }
        public AccountModel.Staff ToDbType()
        {
            return new AccountModel.Staff()
            {
                ID = this.ID,
                UserID = this.UserID,
                Name = this.Name,
                Active = this.Active
            };
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
