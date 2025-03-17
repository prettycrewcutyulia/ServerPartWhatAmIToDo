using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;

namespace ServerPartWhatAmIToDo.Models;
using System.Text.Json.Serialization;

public class GptResponse
{
    [JsonPropertyName("result")]
    public Result Result { get; set; }
}

public class Result
{
    [JsonPropertyName("alternatives")]
    public Alternative[] Alternatives { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }

    [JsonPropertyName("modelVersion")]
    public string ModelVersion { get; set; }
}

public class Alternative
{
    [JsonPropertyName("message")]
    public Message Message { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class Usage
{
    [JsonPropertyName("inputTextTokens")]
    public string InputTextTokens { get; set; }

    [JsonPropertyName("completionTokens")]
    public string CompletionTokens { get; set; }

    [JsonPropertyName("totalTokens")]
    public string TotalTokens { get; set; }

    [JsonPropertyName("completionTokensDetails")]
    public CompletionTokensDetails CompletionTokensDetails { get; set; }
}

public class CompletionTokensDetails
{
    [JsonPropertyName("reasoningTokens")]
    public string ReasoningTokens { get; set; }
}

public class GoalGPT
{

    [JsonProperty("title")]
    public string Title { get; set; }
}

public class StepGPT
{

    [JsonProperty("description")]
    public string Description { get; set; }
}

public class GoalPlan
{
    [JsonProperty("goal")]
    public GoalGPT Goal { get; set; }

    [JsonProperty("steps")]
    public List<StepGPT> Steps { get; set; }
}
