using System;
using System.Text.Json.Serialization;

namespace ValiBot.Entities
{
    public class BaseEntity
    {
        [JsonIgnore]
        public long Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}