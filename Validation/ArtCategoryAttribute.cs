using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Artify.Api.Validation
{
    public class ArtCategoryAttribute : ValidationAttribute
    {
        // Using a static readonly array to keep it synchronized with Frontend
        private static readonly string[] AllowedCategories = new[]
        {
            "Painting & Drawing",
            "Sculpture & Ceramics",
            "Textile & Fashion Design",
            "Architecture & Interior Design",
            "Literature",
            "Music",
            "Film & Theatre",
            "Performing Arts (Dance, Mime, etc.)",
            "Digital Art & Graphic Design",
            "Decorative Arts & Jewelry"
        };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // Assuming it's optional here. Use [Required] attribute if mandotory.
                return ValidationResult.Success;
            }

            var category = value as string;

            if (string.IsNullOrEmpty(category))
            {
                return ValidationResult.Success;
            }

            if (AllowedCategories.Contains(category))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Invalid Category. Allowed values are: {string.Join(", ", AllowedCategories)}");
        }
    }
}
