using Bogus.DataSets;

namespace MentorPlatform.Tests.UnitTests.Identity.API.BogusData
{
    public static class CustomPasswordExtension
    {
        public static string GenerateCustomPassword(this Internet internet)
        {
            var randomizer = internet.Random;

            var number = randomizer.Char('0', '9').ToString(); 
            var upperLetter = randomizer.Char('A', 'Z').ToString();
            var symbol = randomizer.Char((char)33, (char)47);

            var padding = randomizer.String2(randomizer.Number(2, 6)); 

            return new string(randomizer.Shuffle(number + upperLetter + symbol + padding).ToArray());
        }
    }
}