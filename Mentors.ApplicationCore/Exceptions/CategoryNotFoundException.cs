namespace Mentors.ApplicationCore.Exceptions
{
    public class CategoryNotFoundException : ObjectNotFoundException
    {
        private static readonly string _categoriesNotFoundMessage = "No categories were found";
        private static readonly string _categoryNotFoundMessage = "Category with such Id {0} was not found";
        public Guid CategoryId { get; }

        public CategoryNotFoundException() : base(_categoriesNotFoundMessage)
        {
        }

        public CategoryNotFoundException(Guid categoryId) : base(string.Format(_categoryNotFoundMessage, categoryId))
        {
            CategoryId = categoryId;
        }
    }
}