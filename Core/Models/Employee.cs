using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Number { get; set; }

        public string Skills { get; set; } = "Dart";

        public string Email { get; set; }
        public int Role { get; set; } = 3; // 1 = admin, 2 = Kantineleder, 3 = Medarbejder
    }
}