using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NerdStore.Catalog.Application.Dtos
{
    public class ProductDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "The {0} field must have a minimum value of {1}.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Image { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must have a minimum value of {1}.")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must have a minimum value of {1}.")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must have a minimum value of {1}.")]
        public decimal Width { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field must have a minumum value of {1}")]
        public decimal Depth { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }

    }
}
