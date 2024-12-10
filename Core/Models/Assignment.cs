using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Core.Models;


public class Assignment
{
    public int Id { get; set; }  // MongoDB generates this ID

    [Required(ErrorMessage = "Employee is required.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string Note { get; set; }
    
    public bool Status { get; set; }
    
}