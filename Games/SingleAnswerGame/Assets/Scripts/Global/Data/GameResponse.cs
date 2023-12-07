using Newtonsoft.Json;

namespace Global.Data
{
    public class GameResponse
    {
        public int Profile { get; set; }
        public int Task { get; set; }

        [JsonProperty("question_responses")] public QuestionResponse[] Responses { get; set; }
    }
}