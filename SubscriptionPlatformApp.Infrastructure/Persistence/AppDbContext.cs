using Microsoft.EntityFrameworkCore;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Domain.Entities;
using System;

namespace SubscriptionPlatformApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public AppDbContext(DbContextOptions<AppDbContext> options, 
        ITenantContextAccessor tenantContextAccessor) : base(options) 
    {
        _tenantContextAccessor = tenantContextAccessor;
    }

    public DbSet<Users> Users => Set<Users>();
    public DbSet<Tenants> Tenants => Set<Tenants>();
    public DbSet<Memberships> Memberships => Set<Memberships>();
    public DbSet<MemberInvitations> MemberInvitations => Set<MemberInvitations>();
    public DbSet<Addresses> Addresses => Set<Addresses>();
    public DbSet<Subscriptions> Subscriptions => Set<Subscriptions>();
    public DbSet<Payments> Payments => Set<Payments>();
    public DbSet<EmailVerificationTokens> EmailVerificationTokens => Set<EmailVerificationTokens>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tenant < - > Address
        modelBuilder.Entity<Tenants>()
            .HasOne(t => t.TenantAddress)
            .WithMany()
            .HasForeignKey(a => a.TenantAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tenants>()
            .HasOne(t => t.BillingAddress)
            .WithMany()
            .HasForeignKey(a => a.BillingAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Memberships>(e =>
        {
            e.HasKey(x => new { x.TenantId, x.UserId });

            e.HasOne(x => x.Tenant)
                .WithMany(b => b.Memberships)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.UserId, x.TenantId }).IsUnique();
        });

        modelBuilder.Entity<MemberInvitations>(e =>
        {
            e.HasKey(x => x.MemberInvitationId);

            e.HasOne(x => x.Tenant)
                .WithMany(b => b.MemberInvitations)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.InvitedByUser)
                .WithMany(u => u.InvitationsSent)
                .HasForeignKey(x => x.InvitedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.InvitedUser)
                .WithMany(u => u.InvitationsReceived)
                .HasForeignKey(x => x.InvitedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // common indexes
            e.HasIndex(x => new { x.TenantId, x.InvitedEmail });
            e.HasIndex(x => x.InvitedEmail);
            e.HasIndex(x => x.HashedToken).IsUnique();
        });

        modelBuilder.Entity<Subscriptions>()
            .HasOne(s => s.Tenant)
            .WithMany(b => b.Subscriptions)
            .HasForeignKey(s => s.TenantId);

        modelBuilder.Entity<Payments>()
            .HasOne(p => p.Tenant)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.TenantId);

        // Optional Settings

        modelBuilder.Entity<Subscriptions>()
            .HasQueryFilter(x =>
                _tenantContextAccessor.Current != null &&
                x.TenantId == _tenantContextAccessor.Current.TenantId);

        modelBuilder.Entity<Payments>()
            .HasQueryFilter(x =>
                _tenantContextAccessor.Current != null &&
                x.TenantId == _tenantContextAccessor.Current.TenantId);

        modelBuilder.Entity<Memberships>()
            .HasQueryFilter(x =>
                _tenantContextAccessor.Current != null &&
                x.TenantId == _tenantContextAccessor.Current.TenantId);

        modelBuilder.Entity<MemberInvitations>()
            .HasQueryFilter(x =>
                _tenantContextAccessor.Current != null &&
                x.TenantId == _tenantContextAccessor.Current.TenantId);
    }
}
