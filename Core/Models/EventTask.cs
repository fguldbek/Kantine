using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Models
{
    
    public class EventTask

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public List<Assignment> AssignmentList { get; set; } = new List<Assignment>();
        
        public int EmployeeCapacity { get; set; }

        public int EmployeesAssigned { get; set; }

    }
}