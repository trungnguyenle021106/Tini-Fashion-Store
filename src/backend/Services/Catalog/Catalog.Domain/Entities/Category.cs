using BuildingBlocks.Core.Entities;
using BuildingBlocks.Core.Exceptions;


namespace Catalog.Domain.Entities
{
    public class Category : BaseEntity<Guid>
    {
        public string Name { get; private set; } = default!;

        private Category() { }

        public Category(string name)
        {
            Id = Guid.NewGuid();
            UpdateName(name);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}