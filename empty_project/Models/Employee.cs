using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Name length cannot exceed 50 characters.")]
        public string Name { get; set; }
        [Required, Display(Name = "Work Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Must select a department")]
        // Departments enum has int rawValue, number types are by default required, to make it optional, make the prop nullable by suffix ?
        public Departments? Department { get; set; }
    }
}
