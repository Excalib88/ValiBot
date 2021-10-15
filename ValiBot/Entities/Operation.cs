namespace ValiBot.Entities
{
    public class Operation : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public OperationType Type { get; set; }
        public bool IsFinished { get; set; }
        
        public AppUser User { get; set; }
        public long? UserId { get; set; }
        
        public Category Category { get; set; }
        public long? CategoryId { get; set; }
    }
}