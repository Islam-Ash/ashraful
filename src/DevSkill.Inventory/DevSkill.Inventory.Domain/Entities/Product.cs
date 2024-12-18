namespace DevSkill.Inventory.Domain.Entities
{
    public class Product : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public Category Category { get; set; }//nevigation

	}
}
