using System;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalog.Domain.Models
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public decimal Price { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public string Image { get; private set; }
        public int StockQuantity { get; private set; }
        public Dimensions Dimensions { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        protected Product() { }

        public Product(string name, string description, bool active, decimal price, Guid categoryId, DateTime registrationDate, string image, Dimensions dimensions)
        {
            Name = name;
            Description = description;
            Active = active;
            Price = price;
            CategoryId = categoryId;
            RegistrationDate = registrationDate;
            Image = image;
            Dimensions = dimensions;

            Validate();
        }

        public void Activate() => Active = true;

        public void Deactivate() => Active = false;

        public void ChangeCategory(Category category) {
            Category = category;
            CategoryId = Category.Id;
        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }

        public void DebitStock(int quantity)
        {
            StockQuantity -= Math.Abs(quantity);
        }

        public void SupplyStock(int quantity)
        {
            StockQuantity += quantity;
        }

        public bool HasStock(int quantity)
        {
            return StockQuantity >= quantity;
        }

        private void Validate() 
        {
            Validations.EmptyThrowsException(Name, "The 'Name' field of the product cannot be empty.");
            Validations.EmptyThrowsException(Description, "The 'Description' field of the product cannot be empty.");
            Validations.EqualsThrowsException(CategoryId, Guid.Empty, "The 'CategoryId' field of the product cannot be empty.");
            Validations.LessThanThrowsException(Price, 0.01m, "The 'Price' field of the product cannot be less than 0.01.");
            Validations.EmptyThrowsException(Image, "The 'Image' field of the product cannot be empty.");
            Validations.NullThrowsException(Dimensions, "The 'Dimensions' field of the product cannot be null.");
        }
    }
}
