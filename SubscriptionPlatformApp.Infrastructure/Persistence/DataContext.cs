using System;
using Microsoft.EntityFrameworkCore;
using SubscriptionPlatformApp.Domain.Entities;

namespace SubscriptionPlatformApp.Infrastructure.Persistence;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

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

        
    }
}
