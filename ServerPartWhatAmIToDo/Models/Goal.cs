using Newtonsoft.Json;
using ServerPartWhatAmIToDo.Models.Goals;

namespace ServerPartWhatAmIToDo.Models;

public class Goal
{
    public string Id { get; set; }
    
    public string UserId { get; set; }
    public string Title { get; set; }
    
    public List<Filter> Category { get; set; }
    
    public List<Step> Steps { get; set; }
    
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? StartDate { get; set; }
    
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Deadline { get; set; }
    

    public Goal(GoalRequest goal)
    {
        Id = Guid.NewGuid().ToString();
        UserId = goal.UserId;
        Title = goal.Title;
        Category =  goal.Categories.Select(c => new Filter(
            goal.UserId,
            "c.Name",
            "c.ColorHex"
            )).ToList();
        Steps = goal.Steps.Select(c=> new Step(c, Id)).ToList();
        StartDate = goal.StartDate;
        Deadline = goal.Deadline;
    }
}

public class Step
{
    public Step(StepRequest stepRequest, string goalId)
    {
        GoalId = goalId;
        Id = Guid.NewGuid().ToString();
        Title = stepRequest.Title;
        IsCompleted = stepRequest.IsCompleted;
    }

    public string Id { get; set; }
    
    public string GoalId { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

public class UnixDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonToken.Integer)
        {
            long unixTime = (long)reader.Value;
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }

        throw new JsonSerializationException("Unexpected token type when parsing a DateTime.");
    }

    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        if (value.HasValue)
        {
            long unixTime = ((DateTimeOffset)value.Value).ToUnixTimeSeconds();
            writer.WriteValue(unixTime);
        }
        else
        {
            writer.WriteNull();
        }
    }
}