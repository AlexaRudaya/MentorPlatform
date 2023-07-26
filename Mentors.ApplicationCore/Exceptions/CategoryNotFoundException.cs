namespace Mentors.ApplicationCore.Exceptions
{
    public class CategoryNotFoundException : ObjectNotFoundException
    {
        private static readonly string CategoriesNotFoundMessage = "No categories were found";
        private static readonly string CategoryNotFoundMessage = "Category with such Id {0} was not found";
        public Guid CategoryId { get; }

        public CategoryNotFoundException() : base(CategoriesNotFoundMessage)
        {
        }

        public CategoryNotFoundException(Guid categoryId) : base(string.Format(CategoryNotFoundMessage, categoryId))
        {
            CategoryId = categoryId;
        }

        public CategoryNotFoundException(string message) : base(message)
        {
        }
    }
}