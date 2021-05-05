
using NerdStore.Catalog.Domain.Models;
using NerdStore.Core.DomainObjects;
using System;
using Xunit;

namespace NerdStore.Catalog.Domain.Tests.Models
{
    public class ProductTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Instantiate_Product_With_EmptyOrNull_Name_ThrowsException(string name)
        {
            var exception = Assert.Throws<DomainException>(() =>
            new Product(name, "Black shirt", true, 59.99m, Guid.NewGuid(), DateTime.Now, "www.site.com/images/shirt.png", new Dimensions(1, 1, 1)));

            Assert.Equal("The 'Name' field of the product cannot be empty.", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Instantiate_Product_With_EmptyOrNull_Description_ThrowsException(string description)
        {
            var exception = Assert.Throws<DomainException>(() =>
            new Product("Shirt", description, true, 59.99m, Guid.NewGuid(), DateTime.Now, "www.site.com/images/shirt.png", new Dimensions(1, 1, 1)));

            Assert.Equal("The 'Description' field of the product cannot be empty.", exception.Message);
        }

        [Fact]
        public void Instantiate_Product_With_Empty_CategoryId_ThrowsException()
        {
            var exception = Assert.Throws<DomainException>(() =>
            new Product("Shirt", "Black shirt", true, 59.99m, Guid.Empty, DateTime.Now, "www.site.com/images/shirt.png", new Dimensions(1, 1, 1)));

            Assert.Equal("The 'CategoryId' field of the product cannot be empty.", exception.Message);
        }

        [Theory]
        [InlineData(0.001)]
        [InlineData(-1)]
        public void Instantiate_Product_With_Price_LessThan_0_01_ThrowsException(decimal price)
        {
            var exception = Assert.Throws<DomainException>(() =>
            new Product("Shirt", "Black shirt", true, price, Guid.NewGuid(), DateTime.Now, "www.site.com/images/shirt.png", new Dimensions(1, 1, 1)));

            Assert.Equal("The 'Price' field of the product cannot be less than 0.01.", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Instantiate_Product_With_EmptyOrNull_Image_ThrowsException(string image)
        {
            var exception = Assert.Throws<DomainException>(() =>
            new Product("Shirt", "Black shirt", true, 59.99m, Guid.NewGuid(), DateTime.Now, image, new Dimensions(1, 1, 1)));

            Assert.Equal("The 'Image' field of the product cannot be empty.", exception.Message);
        }

        [Fact]
        public void Instantiate_Product_With_Null_Dimensions_ThrowsException()
        {
            var exception = Assert.Throws<DomainException>(() =>
            new Product("Shirt", "Black shirt", true, 59.99m, Guid.NewGuid(), DateTime.Now, "www.site.com/images/shirt.png", null));

            Assert.Equal("The 'Dimensions' field of the product cannot be null.", exception.Message);
        }
    }
}
