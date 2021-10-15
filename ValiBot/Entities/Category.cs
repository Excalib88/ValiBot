using System.Text.Json.Serialization;

namespace ValiBot.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public OperationType Type { get; set; }
        
        [JsonIgnore]
        public AppUser User { get; set; }
        public long? UserId { get; set; }
    }
}