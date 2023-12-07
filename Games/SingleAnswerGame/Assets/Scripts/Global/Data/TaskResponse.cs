using Newtonsoft.Json;

namespace Global.Data
{
    public class TaskResponse
    {
        [JsonProperty("question_responses")] public QuestionResponse[] Responses { get; set; }
    }
}