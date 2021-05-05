using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalog.Domain.Models
{
    public class Dimensions : ValueObject
    {
        public decimal Height { get; private set; }
        public decimal Width { get; private set; }
        public decimal Depth { get; private set; }

        public Dimensions(decimal height, decimal width, decimal depth)
        {
            Height = height;
            Width = width;
            Depth = depth;

            Validate();
        }

        public string FormattedDimensions()
        {
            return $"HxWxD: {Height} x {Width} x {Depth}";
        }

        public override string ToString()
        {
            return FormattedDimensions();
        }

        public void Validate()
        {
            Validations.LessThanThrowsException(Height, 1, "The 'Height' field of the dimensions cannot be less than or equal to 0.");
            Validations.LessThanThrowsException(Width, 1, "The 'Width' field of the dimensions cannot be less than or equal to 0.");
            Validations.LessThanThrowsException(Depth, 1, "The 'Depth' field of the dimensions cannot be less than or equal to 0.");
        }
    }
}
