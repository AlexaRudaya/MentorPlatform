using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.ModelConfiguration
{
    public class MessageModelConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasOne(message => message.User)
                .WithMany(user => user.Messages)
                .HasForeignKey(message => message.UserId);
        }
    }
}