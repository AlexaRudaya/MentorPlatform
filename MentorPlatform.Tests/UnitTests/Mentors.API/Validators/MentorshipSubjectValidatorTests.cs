namespace MentorPlatform.Tests.UnitTests.Mentors.API.Validators
{
    public class MentorshipSubjectValidatorTests
    {
        private readonly MentorshipSubjectValidator _subjectValidator;
        private readonly MentorshipSubjectGenerator _subjectData;

        public MentorshipSubjectValidatorTests() 
        {
            _subjectValidator = new MentorshipSubjectValidator();
            _subjectData = new MentorshipSubjectGenerator();
        }

        [Fact]
        public async Task ValidateMentorshipSubjectDto_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var subjectDto = _subjectData.GenerateFakeDto();

            // Act
            var result = await _subjectValidator.TestValidateAsync(subjectDto);

            // Assert
            result
                .ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("CV")]
        public async Task ValidateMentorshipSubjectDto_InvalidValues_ShouldFailValidation(string invalidName)
        {
            // Arrange
            var subjectDto = _subjectData.GenerateFakeDto();
            subjectDto.Name = invalidName;

            // Act
            var result = await _subjectValidator.TestValidateAsync(subjectDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(subjectDto => subjectDto.Name);
        }
    }
}