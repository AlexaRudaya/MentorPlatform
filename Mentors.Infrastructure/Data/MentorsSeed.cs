namespace Mentors.Infrastructure.Data
{
    public sealed class MentorsSeed
    {
        public static async Task SeedAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var mentorDbContext = scope.ServiceProvider.GetRequiredService<MentorDbContext>();

            if (!mentorDbContext.Mentors.Any())
            {
                await mentorDbContext.Database.MigrateAsync();
                SeedMentors(mentorDbContext);
            }

            SeedMentors(mentorDbContext);
        }

        private static void SeedMentors(MentorDbContext mentorDbContext)
        {
            var mentors = GetPreConfiguredMentors(mentorDbContext);
            mentorDbContext.Mentors.AddRange(mentors);
            mentorDbContext.SaveChanges();
        }

        private static IEnumerable<Mentor> GetPreConfiguredMentors(MentorDbContext mentorDbContext)
        {
            var mentors = new List<Mentor>
            {
                new Mentor
                {
                    Name = "Jason Caleb",
                    Biography = "Experienced Software Developer adept in bringing forth expertise in design, installation, testing and maintenance of large scale software systems. My career of over 9 years as a software developer has accomplished an expertise in building distributed highly scalable systems serving millions of users across the world",
                    HourlyRate = 200,
                    MeetingDuration = 60,
                    Category = new Category()
                    {
                       Name = "Backend Development on Java"
                    },
                    Availabilities = new List<Availability>()
                    {
                        new Availability
                        {
                            Date = DateTime.UtcNow.Date,
                            IsAvailable = true,
                            StartTime = DateTime.UtcNow.Date.AddHours(4),
                            EndTime = DateTime.UtcNow.Date.AddHours(15)
                        },
                        new Availability
                        {
                            Date = DateTime.UtcNow.Date,
                            IsAvailable = true,
                            StartTime = DateTime.UtcNow.Date.AddHours(8),
                            EndTime = DateTime.UtcNow.Date.AddHours(10)
                        }
                    }
                },
                new Mentor
                {
                    Name = "Kris Willy",
                    Biography = "I’m a data scientist with 10+ years of experience. My journey started at particle physics, where I enjoyed working at the largest experiments with cutting edge technologies. Afterwards I transitioned to a software company, which creates B2B SaaS solutions for customers in the Supply Chain industry.",
                    HourlyRate = 250,
                    MeetingDuration = 60,
                    Category = new Category()
                    {
                       Name = "Data Science"
                    },
                    Availabilities = new List<Availability>()
                    {
                        new Availability
                        {
                            Date = DateTime.UtcNow.Date,
                            IsAvailable = true,
                            StartTime = DateTime.UtcNow.Date.AddHours(2),
                            EndTime = DateTime.UtcNow.Date.AddHours(10)
                        },
                        new Availability
                        {
                            Date = DateTime.UtcNow.Date,
                            IsAvailable = true,
                            StartTime = DateTime.UtcNow.Date.AddHours(4),
                            EndTime = DateTime.UtcNow.Date.AddHours(12)
                        }
                    }
                },
                 new Mentor
                {
                    Name = "Lilly Polly",
                    Biography = "I am a Senior UX/UI Designer with 16 years of experience. I am also a UX/UI Design Instructor at allWomen.tech and a mentor at UXDI and DesignLab. I have worked in different business areas (energy, travel, secondhand market, healthcare) and in different types of companies",
                    HourlyRate = 220,
                    MeetingDuration = 60,
                    Category = new Category()
                    {
                       Name = "UX/UI"
                    },
                    Availabilities = new List<Availability>()
                    {
                        new Availability
                        {
                             Date = DateTime.UtcNow.Date,
                             IsAvailable = true,
                             StartTime = DateTime.UtcNow.Date.AddHours(8),
                             EndTime = DateTime.UtcNow.Date.AddHours(15)
                        },
                        new Availability
                        {
                             Date = DateTime.UtcNow.Date,
                             IsAvailable = true,
                             StartTime = DateTime.UtcNow.Date.AddHours(10),
                             EndTime = DateTime.UtcNow.Date.AddHours(16)
                        }
                    }
                },
                new Mentor
                {
                    Name = "Adam Dory",
                    Biography = "Customer-obsessed, value-driven product manager with a proven track record of approaching the product development lifecycle as holistic, simultaneous, problem-solving of both user and business goals. Hands-on leader enthusiastic about making an impact by creating a transparent vision, while supporting team collaboration and a culture of innovation. ",
                    HourlyRate = 300,
                    MeetingDuration = 60,
                     Category = new Category()
                    {
                       Name = "Project management"
                    },
                    Availabilities = new List<Availability>()
                    {
                        new Availability
                        {
                            Date = DateTime.UtcNow.Date,
                            IsAvailable = true,
                            StartTime = DateTime.UtcNow.Date.AddHours(3),
                            EndTime = DateTime.UtcNow.Date.AddHours(13)
                        }
                    }
                }
            };

            return mentors;
        }
    }
}