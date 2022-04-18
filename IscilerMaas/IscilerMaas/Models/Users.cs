using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IscilerMaas.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Username { get; set; }
        public string Sifre { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
