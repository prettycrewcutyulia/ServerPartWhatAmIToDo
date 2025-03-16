namespace ServerPartWhatAmIToDo.Models;

public class Filter
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
}

public class UpdateFilterRequest
{
    public string Title { get; set; }
    public string Color { get; set; }
}