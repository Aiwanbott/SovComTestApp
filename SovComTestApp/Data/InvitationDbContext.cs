using Microsoft.EntityFrameworkCore;
using SovComTestApp.Models;

namespace SovComTestApp.Data;

public class InvitationDbContext : DbContext
{
    public InvitationDbContext(DbContextOptions<InvitationDbContext> options) : base(options)
    {
    }
    public DbSet<Invitation> Invitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.ToTable("Invitations");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.CreatedDate)
                .IsRequired(true)
                .HasColumnType("date");

            entity.Property(e => e.PhoneNumber)
                .IsRequired(true)
                .HasMaxLength(16);

            entity.Property(e => e.ApiId)
                .HasDefaultValue(4);

            entity.Property(e => e.Message)
                .IsRequired(true)
                .HasMaxLength(160);
        });
    }
}

