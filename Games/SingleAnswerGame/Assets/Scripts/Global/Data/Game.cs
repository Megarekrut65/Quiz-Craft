using Newtonsoft.Json;

namespace Global.Data
{
    public class Game
    {
        public string Type { get; set; }
        
        [JsonProperty("min_win_grade")]
        public int MinWinGrade { get; set; }
        
        [JsonProperty("player_weapon")]
        public string PlayerWeapon { get; set; }
        
        [JsonProperty("enemy_weapon")]
        public string EnemyWeapon { get; set; }
        
        public Task Task { get; set; }
    }
}