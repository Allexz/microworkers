using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Entities;

namespace Microworkers.Infrastructure.Data.Mappers;
internal class UserMapConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("Id").HasColumnType("uuid")
            .IsRequired();

        builder.Property<string>(x => x.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(75);


        builder.Property<string>(x => x.Password)
            .HasColumnName("Password")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property<string>(x => x.Username)
           .HasColumnName("Username")
           .IsRequired()
           .HasMaxLength(100);

        builder.OwnsOne(x => x.Document, document =>
        {
            document.Property(p => p.Number)
                .HasColumnName("Document")
                .IsRequired()
                .HasMaxLength(30);

            document.Property(p => p.DocType)
                .HasColumnName("DocumentType")
                .IsRequired()
                .HasConversion<string>( );
        });

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

            address.Property<string?>(a => a.Additional)
                .HasColumnName("Additional")
                .HasMaxLength(30);
        });

        builder.HasMany(u => u.Skills)           // User tem muitas Skills
       .WithMany(s => s.Users)           // Skill tem muitos Users
       .UsingEntity<Dictionary<string, object>>( // Tabela de junção
           "UserSkills",                  // Nome da tabela
           j => j.HasOne<Skill>()         // Configuração do lado Skill
                 .WithMany()
                 .HasForeignKey("SkillId")
                 .OnDelete(DeleteBehavior.Cascade),
           j => j.HasOne<User>()         // Configuração do lado User
                 .WithMany()
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade),
           j => j.HasKey("UserId", "SkillId") // Chave composta
       );

        // Taskis mapping (as relationship)
        builder.HasMany(x => x.Taskis)
            .WithOne()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);

        // Proposals mapping (as relationship)
        builder.HasMany(x => x.Proposals)
            .WithOne()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);

    }
}
