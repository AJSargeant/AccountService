using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountModel
{
    public class AccountContext : DbContext
    {

        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public AccountContext() : base(){}
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
