using Newtonsoft.Json;

namespace Global.Data
{
    public class QuestionResponse
    {
        public int Question { get; set; }
        public int Answer { get; set; }

        [JsonProperty("text_answer")] public string TextAnswer { get; set; }
    }
}