using MongoDB.Bson;

namespace Core.Models;


public class Assignment
{
    public int Id { get; set; }  // MongoDB will automatically generate this ID

    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public bool Status { get; set; }
    
}