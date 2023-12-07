using Newtonsoft.Json;

namespace Global.Data
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [JsonProperty("created_by")] public int CreatedBy { get; set; }

        public Question[] Questions { get; set; }
    }
}