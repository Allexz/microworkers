using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microworkers.Domain.Core.Aggregates;

namespace Microworkers.Infrastructure.Data.Mappers;
internal class ProposalMapConfiguration : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.ToTable("Proposals");
        builder.HasKey(p => p.Id);
    
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
    
            builder.Property(p => p.TaskId)
                .IsRequired()
                .HasColumnType("uuid");
    
            builder.Property(p => p.ServiceProviderId)
                .IsRequired().HasColumnType("uuid");
    
            builder.OwnsOne(p => p.JobDescription, jobDesc =>
            {
                jobDesc.Property(jd => jd.Description)
                    .IsRequired()
                    .HasMaxLength(500);
            });
    
            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>();
    
            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
    
            builder.Property(p => p.ProposalDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP()");
    
            builder.Property(p => p.ProjectedDate)
                .IsRequired();
    }
}
