﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AccountModel
{
    public class User
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}
