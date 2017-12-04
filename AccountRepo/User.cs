using System;
using System.Collections.Generic;
using System.Text;

namespace AccountRepo
{
    public class User
    {
        public User(AccountModel.User u)
        {
            ID = u.ID;
            UserID = u.UserID;
            Name = u.Name;
            Email = u.Email;
            Authorised = u.Authorised;
            Active = u.Active;
        }

        public AccountModel.User ToDbType()
        {
            return new AccountModel.User()
            {
                ID = this.ID,
                UserID = this.UserID,
                Name = this.Name,
                Email = this.Email,
                Authorised = this.Authorised,
                Active = this.Active
            };
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Authorised { get; set; }
        public bool Active { get; set; }
    }
}
