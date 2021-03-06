﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccountModel
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Authorised { get; set; }
        public bool Active { get; set; }
    }
}
