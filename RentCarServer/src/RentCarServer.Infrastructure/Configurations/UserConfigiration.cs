using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Users;
using RentCarServer.Domain.Users.ValueObjects;

namespace RentCarServer.Infrastructure.Configurations;

public sealed class UserConfigiration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value));

        builder.Property(p => p.FirstName)
            .HasConversion(
                firstName => firstName.Value,
                value => new FirstName(value));

        builder.Property(p => p.LastName)
            .HasConversion(
                lastName => lastName.Value,
                value => new LastName(value));

        builder.Property(p => p.UserName)
            .HasConversion(
                userName => userName.Value,
                value => new UserName(value));

        builder.OwnsOne(p => p.Password, passwordBuilder =>
        {
            passwordBuilder.Property(p => p.PasswordHash).HasColumnName("PasswordHash");
            passwordBuilder.Property(p => p.PasswordSalt).HasColumnName("PasswordSalt");
        });

        builder.Ignore(p => p.FullName);
    }
}