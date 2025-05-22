using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microworkers.Domain.Core.Aggregates;

namespace Microworkers.Infrastructure.Data.Mappers;
internal class UserMapConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property<string>(x => x.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(75);

        builder.Property<string>(x => x.Document)
            .HasColumnName("Document")
            .IsRequired()
            .HasMaxLength(14);

        builder.Property<string>(x => x.Password)
            .HasColumnName("Password")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property<string>(x => x.Username)
           .HasColumnName("Username")
           .IsRequired()
           .HasMaxLength(100);

        builder.OwnsOne(x => x.Phone, phone =>
        {
            phone.Property(p => p.Number)
                .HasColumnName("Phone")
                .IsRequired()
                .HasMaxLength(15);
        });

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property<string>(a => a.State)
                .HasColumnName("State")
                .IsRequired()
                .HasMaxLength(2);

            address.Property<string>(a => a.Street)
                .HasColumnName("Zipcode")
                .IsRequired()
                .HasMaxLength(100);

            address.Property<string>(a => a.City)
                .HasColumnName("City")
                .IsRequired()
                .HasMaxLength(75);

            address.Property<string>(a => a.NeighborHood)
                .HasColumnName("NeighborHood")
                .IsRequired()
                .HasMaxLength(30);

            address.Property<string>(a => a.Street)
                .HasColumnName("Street")
                .IsRequired()
                .HasMaxLength(75);

            address.Property<string>(a => a.Number)
                .HasColumnName("Number")
                .IsRequired()
                .HasMaxLength(10);

            address.Property<string>(a => a.Additional)
                .HasColumnName("Additional")
                .HasMaxLength(30);
        });

        builder.HasMany(x => x.Skills)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
