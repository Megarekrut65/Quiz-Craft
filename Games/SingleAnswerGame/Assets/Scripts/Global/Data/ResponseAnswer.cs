using Newtonsoft.Json;

namespace Global.Data
{
    public class ResponseAnswer
    {
        [JsonProperty("answer_id")]
        public int AnswerId { get; set; }
        public bool Correct { get; set; }
    }
}