using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Infrastructure.Data.Mappers;

internal class TaskMapConfiguration : IEntityTypeConfiguration<Taski>
{
    public void Configure(EntityTypeBuilder<Taski> builder)
    {
        builder.ToTable("Taskis");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Id").HasColumnType("uuid")
            .IsRequired();

        builder.Property(t => t.CustomerId)
            .HasColumnName("Customer").HasColumnType("uuid")
            .IsRequired();

        builder.Property(t => t.ServiceProviderId)
            .HasColumnName("ServiceProvider").HasColumnType("uuid")
            .IsRequired();

        builder.Property(t => t.RequiredSkill)
            .HasColumnName("RequiredSkill").HasColumnType("uuid")
            .IsRequired();

        builder.Property(t => t.RequestDate)
            .HasColumnName("RequestDate")
            .HasDefaultValue("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.OwnsOne(t => t.Description, desc =>
        {
            desc.Property(d => d.Value)
                .HasColumnName("Description")
                .IsRequired()
                .HasMaxLength(500);
        });

        builder.Property(t => t.Status)
            .HasColumnName("Status")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Result)
            .HasColumnName("Result")
            .HasConversion<string>();

        builder.Property(t => t.CompletionDate)
            .HasColumnName("CompletionDate");

        builder.OwnsMany(typeof(TaskiUpdate), "_updates", updates =>
        {
            updates.ToTable("TaskiUpdates");
            updates.WithOwner().HasForeignKey("TaskiId");
            updates.Property<int>("Id").HasColumnName("Id").HasColumnType("uuid").IsRequired();
            updates.Property<Guid>("UserId").HasColumnName("UserId").HasColumnType("uuid").IsRequired();
            updates.Property<string>("Message").HasColumnName("Message").IsRequired().HasMaxLength(500);
            updates.Property<DateTime>("Timestamp").HasColumnName("Timestamp").IsRequired();
            updates.HasKey("TaskiId", "Id");
        });


    }
}