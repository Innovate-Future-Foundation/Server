using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InnovateFuture.Infrastructure.Orders.Persistence.ModelConfigs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // set primary key
            builder.HasKey(x => x.User_id);

            // set properties
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

        }
    }
}