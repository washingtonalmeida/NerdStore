using NerdStore.Catalog.Domain.Models;
using NerdStore.Core.DomainObjects;
using Xunit;

namespace NerdStore.Catalog.Domain.Tests.Models
{
    public class DimensionsTests
    {

        [Theory]
        [InlineData(0.9)]
        [InlineData(-1)]
        public void Instantiate_Dimensions_With_Height_LessThan_1_ThrowsException(decimal height)
        {
            var exception = Assert.Throws<DomainException>(() => new Dimensions(height, 1, 1));

            Assert.Equal("The 'Height' field of the dimensions cannot be less than or equal to 0.", exception.Message);
        }

        [Theory]
        [InlineData(0.9)]
        [InlineData(-1)]
        public void Instantiate_Dimensions_With_Width_LessThan_1_ThrowsException(decimal width)
        {
            var exception = Assert.Throws<DomainException>(() => new Dimensions(1, width, 1));

            Assert.Equal("The 'Width' field of the dimensions cannot be less than or equal to 0.", exception.Message);
        }

        [Theory]
        [InlineData(0.9)]
        [InlineData(-1)]
        public void Instantiate_Dimensions_With_Depth_LessThan_1_ThrowsException(decimal depth)
        {
            var exception = Assert.Throws<DomainException>(() => new Dimensions(1, 1, depth));

            Assert.Equal("The 'Depth' field of the dimensions cannot be less than or equal to 0.", exception.Message);
        }

    }
}
