using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportTicket.Api.Models;
using SupportTicket.Api.Models.Enums;

namespace SupportTicket.Api.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(2000);

        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(TicketStatus.Open);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_Tickets_Status");

        builder.HasIndex(t => t.Title)
            .HasDatabaseName("IX_Tickets_Title");

        builder.HasIndex(t => t.CreatedById)
            .HasDatabaseName("IX_Tickets_CreatedById");

        builder.HasIndex(t => t.AssignedToId)
            .HasDatabaseName("IX_Tickets_AssignedToId");

        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.TicketsCreated)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Tickets_CreatedBy_Users");

        builder.HasOne(t => t.AssignedTo)
            .WithMany(u => u.TicketsAssigned)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Tickets_AssignedTo_Users");
    }
}
