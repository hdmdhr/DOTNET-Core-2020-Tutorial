using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [DisplayName("New Role Name")]
        public string RoleName { get; set; }
    }
}
