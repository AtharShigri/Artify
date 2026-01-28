using Artify.Api.Models;

namespace Artify.Api.Extensions
{
    public static class CategoryExtensions
    {
        public static bool IsNullOrEmpty(this Category? category)
        {
            if (category == null)
                return true;

            return string.IsNullOrWhiteSpace(category.Name);
        }
    }
}
