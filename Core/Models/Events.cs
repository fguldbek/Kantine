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
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [Required]
        public string Location { get; set; }
        [Required]
        public string Food { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Participants must be greater than 0.")]
        public int Participants { get; set; }
        public string Requests { get; set; }
        
        public Company Company { get; set; }
        [Required]
        public string? ImageUrl { get; set; }

        
        public List<EventTask> TaskList { get; set; } = new List<EventTask>();
    }
}