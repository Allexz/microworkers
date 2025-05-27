using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microworkers.Domain.Core.Entities;

namespace Microworkers.Infrastructure.Data.Mappers;
internal class SkillMapConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("Skills");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("Id")
            .IsRequired().HasColumnType("uuid");
        builder.Property(s => s.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(100);
    }
}
