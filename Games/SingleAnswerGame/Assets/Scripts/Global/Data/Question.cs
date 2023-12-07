using Newtonsoft.Json;

namespace Global.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        [JsonProperty("max_grade")] public int MaxGrade { get; set; }

        public Answer[] Answers { get; set; }
    }
}