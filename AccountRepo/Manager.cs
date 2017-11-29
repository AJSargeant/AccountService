using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccountRepo
{
    public class Manager
    {
        public Manager(AccountModel.Manager m)
        {
            ID = m.ID;
            UserID = m.UserID;
            Name = m.Name;
            Active = m.Active;
        }

        public AccountModel.Manager ToDbType()
        {
            return new AccountModel.Manager()
            {
                ID = this.ID,
                UserID = this.UserID,
                Name = this.Name,
                Active = this.Active
            };
        }

        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
