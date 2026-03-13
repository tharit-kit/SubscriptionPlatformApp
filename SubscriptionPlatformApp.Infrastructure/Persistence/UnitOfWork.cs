using Microsoft.EntityFrameworkCore.Storage;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Infrastructure.Repositories;

namespace SubscriptionPlatformApp.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _tx;
    private bool _disposed;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        User = new UserRepository(_context);
        Address = new AddressRepository(_context);
        EmailVerificationToken = new EmailVerificationTokenRepository(_context);
        MemberInvitation = new MemberInvitationRepository(_context);
        MemberMembership = new MembershipRepository(_context);
        Subscription = new SubscriptionRepository(_context);
        Payment = new PaymentRepository(_context);
        Tenant = new TenantRepository(_context);
    }

    public IUserRepository User { get; }
    public IAddressRepository Address { get; }
    public IEmailVerificationTokenRepository EmailVerificationToken { get; }
    public IMemberInvitationRepository MemberInvitation { get; }
    public IMembershipRepository MemberMembership { get; }
    public ISubscriptionRepository Subscription { get; }
    public IPaymentRepository Payment { get; }
    public ITenantRepository Tenant { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _tx = await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
        if (_tx != null) await _tx.CommitAsync(ct);
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_tx != null) await _tx.RollbackAsync(ct);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
