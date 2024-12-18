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
    public class Events
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public DateTime StartDate { get; set; } = DateTime.Now;
        
        public string Location { get; set; }
        
        public string Food { get; set; }
        
        public int Participants { get; set; }
        
        public string Requests { get; set; }
        
        public Company Company { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public List<EventTask> TaskList { get; set; } = new List<EventTask>();
    }
}