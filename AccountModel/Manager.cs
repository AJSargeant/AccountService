using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccountModel
{
    public class Manager
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
