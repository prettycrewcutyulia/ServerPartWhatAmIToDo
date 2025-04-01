using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServerPartWhatAmIToDo.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase
{

    private readonly IGoalService _goalService;
    public GoalsController(IGoalService goalService)
    {
        _goalService = goalService;
    }
    [HttpGet]
    public IActionResult GetAllGoals([FromQuery] int userId)
    {
        // Фильтрация целей по userId
        var goals = _goalService.GetGoalsByUserIdAsync(userId).Result;
        return Ok(goals);
    }

    [HttpPost("create")]
    public IActionResult CreateGoal([FromBody] GoalRequest newGoal)
    {
        _goalService.AddGoalAsync(newGoal).Wait();
        return Ok(new { Message = "Goal created successfully" });
    }
    
    [HttpPost("get-ai")]
    public async Task<IActionResult> GetGoalUsingAI([FromBody] AiGoalRequest request)
    {
        var response = await Yandex_GPT(request.Context);
        return Ok(response);
    }


    [HttpPut("update/{id}")]
    public IActionResult UpdateGoal(int id, [FromBody] GoalRequest request)
    {
        _goalService.UpdateGoalAsync(id, request).Wait();

        return Ok(new { Message = "Goal updated successfully" });
    }
    
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteGoal(int id)
    {
       _goalService.DeleteGoalAsync(id).Wait();
        return Ok(new { Message = "Goal deleted successfully" });
    }
    
    private async Task<GoalPlan?> Yandex_GPT(string message)
    { 
        // Загрузите переменные окружения из .env файла
        DotNetEnv.Env.Load();

        // Получите токен из переменных окружения
        string token = Environment.GetEnvironmentVariable("OAUTH_TOKEN");
        string gptUrl = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";
        string iamToken = await GetIamToken(token);
            
        var client = new HttpClient();
            

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {iamToken}");
                
        var jsonText = await System.IO.File.ReadAllTextAsync("prompt.json");
        
        string updatedJson = ReplaceMessageText(jsonText, message);
        // Сериализуем тело в JSON
        var jsonContent = new StringContent(updatedJson, Encoding.UTF8, "application/json");

        int maxRetries = 3; // Максимальное количество попыток
        int retryCount = 0;
        while (retryCount < maxRetries)
        {
            try
            {
                // Отправляем POST запрос
                var response = await client.PostAsync(gptUrl, jsonContent);

                // Получаем ответ от нейросети
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && responseBody != null)
                {
                    GptResponse result = JsonSerializer.Deserialize<GptResponse>(responseBody);

                    // Десериализация внутреннего JSON в конечную модель
                    GoalPlan goalPlan =
                        JsonConvert.DeserializeObject<GoalPlan>(result.Result.Alternatives[0].Message.Text);
                    return goalPlan;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode} - {responseBody}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Увеличиваем счетчик попыток и ждем перед повтором
                retryCount++;
                await Task.Delay(3); // Задержка между запросами
                Console.WriteLine($"Ошибка при отправке запроса: {ex.Message}");
            }
            
            if (retryCount < maxRetries)
            {
                iamToken = await GetIamToken(token);
                client.DefaultRequestHeaders.Remove("Authorization");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {iamToken}");
            }
        }
        
        return null;
    }   
    
    private async Task<string> GetIamToken(string oauthToken)
    {
        using var client = new HttpClient();
        var requestContent = new StringContent($"{{\"yandexPassportOauthToken\": \"{oauthToken}\"}}", 
            System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://iam.api.cloud.yandex.net/iam/v1/tokens", requestContent);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(responseBody);
        return jsonDoc.RootElement.GetProperty("iamToken").GetString();
    }
    
    private string ReplaceMessageText(string json, string newText)
    {
        DotNetEnv.Env.Load();

        // Получите токен из переменных окружения
        string newModelUri = Environment.GetEnvironmentVariable("MODELURI");
        try
        {
            // Парсим JSON строку
            var jObject = JObject.Parse(json);
            
            // Обновляем параметр modelUri
            if (jObject["modelUri"] != null)
            {
                jObject["modelUri"] = newModelUri;
            }
            
            // Доступ к массиву messages
            var messages = jObject["messages"] as JArray;
            if (messages != null && messages.Count > 0)
            {
                // Доступ к первому объекту message
                var firstMessage = messages[0] as JObject;
                if (firstMessage != null)
                {
                    // Замена текста в свойстве text
                    firstMessage["text"] = newText;
                }
            }

            // Преобразуем обратно в строку JSON
            return jObject.ToString(Formatting.Indented);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating JSON: " + ex.Message);
            return json; // Возвращаем оригинальный JSON в случае ошибки
        }
    }
}
