using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasIndex(c => new { c.TicketId, c.CreatedAt })
            .HasDatabaseName("IX_Comments_TicketId_CreatedAt");

        builder.HasOne(c => c.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Comments_Ticket_Tickets");

        builder.HasOne(c => c.CreatedBy)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Comments_CreatedBy_Users");
    }
}
