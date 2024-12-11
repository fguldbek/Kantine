namespace Core.Models;

public class Communication
{
    
    public int Id { get; set; }
    
    public int IdOfSender { get; set; }
    
    public int IdOfReceiver { get; set; }
    
    public string Title { get; set; }
    
    public string Message { get; set; }
}
    
