using System.Collections.Generic;
using Newtonsoft.Json;

namespace Main.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        
        [JsonProperty("max_grade")]
        public int MaxGrade { get; set; }
        public List<Answer> Answers { get; set; }
    }
}