namespace ServerPartWhatAmIToDo.Models;

public class Filter
{
    public Filter(string userId, string title, string color)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        Title = title;
        Color = color;
    }

    public string Id { get; set; }
    
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
}