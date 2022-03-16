using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IscilerMaas.Models
{
    public class Isciler
    {

        public int? IsciId { get; set; }

        [Required(ErrorMessage = "Ad daxil olunmalıdır!")]
        public string IsciName { get; set; }

        public int DepId { get; set; }
        public string DepName { get; set; }
        public string Sifre { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Role> Roles { get; set; }
        public List<Department> Departments { get; set; }
        public bool TrueFalse { get; set; }
        public List<Odenis> Odenisler { get; set; }

        public int LoginRole { get; set; }
        
    }
}
