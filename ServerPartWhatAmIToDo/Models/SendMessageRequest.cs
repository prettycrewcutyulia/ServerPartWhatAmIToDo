namespace ServerPartWhatAmIToDo.Models;

public class SendMessageRequest
{
    public long ChatId { get; set; }
    public string Message { get; set; }

    public SendMessageRequest(long chatId, string message)
    {
        ChatId = chatId;
        Message = message;
    }
}