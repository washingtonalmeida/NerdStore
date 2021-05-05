using NerdStore.Core.DomainObjects;
using System.Collections.Generic;

namespace NerdStore.Catalog.Domain.Models
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        public int Code { get; private set; }

        // EF Relation Only
        public ICollection<Product> Products { get; set; }
        protected Category() { }

        public Category(string name, int code)
        {
            Name = name;
            Code = code;
            
            Validate();
        }

        public override string ToString()
        {
            return $"{Name} - {Code}";
        }

        private void Validate()
        {
            Validations.EmptyThrowsException(Name, "The 'Name' field of the category cannot be empty.");
            Validations.LessThanThrowsException(Code, 1, "The 'Code' field of the category cannot be less than 1.");
        }

    }
}
