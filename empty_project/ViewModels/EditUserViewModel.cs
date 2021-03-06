﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            // init list to avoid NullReferenceException
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }

        public IList<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
