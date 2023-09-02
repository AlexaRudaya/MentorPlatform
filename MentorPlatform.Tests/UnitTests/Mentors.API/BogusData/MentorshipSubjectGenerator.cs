﻿using Mentors.Domain.Entities.MongoDb;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.BogusData
{
    public class MentorshipSubjectGenerator
    {
        public MentorshipSubject GenerateFakeMentorshipSubject() 
        {
            return new Faker<MentorshipSubject>()
                .RuleFor(subject => subject.Id, faker => faker.Random.String2(24))
                .RuleFor(subject => subject.Name, faker => faker.Commerce.Department())
                .Generate();
        }
    }
}