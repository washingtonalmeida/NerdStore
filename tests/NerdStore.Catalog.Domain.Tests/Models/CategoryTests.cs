using NerdStore.Catalog.Domain.Models;
using NerdStore.Core.DomainObjects;
using Xunit;

namespace NerdStore.Catalog.Domain.Tests.Models
{
    public class CategoryTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Instantiate_Category_With_EmptyOrNull_Name_ThrowsException(string name)
        {
            var exception = Assert.Throws<DomainException>(() => new Category(name, 1));

            Assert.Equal("The 'Name' field of the category cannot be empty.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Instantiate_Category_With_Code_LessThan_1_ThrowsException(int code)
        {
            var exception = Assert.Throws<DomainException>(() => new Category("Clothing", code));

            Assert.Equal("The 'Code' field of the category cannot be less than 1.", exception.Message);
        }
    }
}
