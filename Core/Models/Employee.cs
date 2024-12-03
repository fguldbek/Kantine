using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "EmployeeId is required")]
        public int EmployeeId { get; set; }
        
        [Required(ErrorMessage = "Name is required.")] 
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string Number { get; set; }

        public string Skills { get; set; } = "Dart";
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address.")]
        public string Email { get; set; }
        public int Role { get; set; } = 3; // 1 = admin, 2 = Kantineleder, 3 = Medarbejder
    }
    
}