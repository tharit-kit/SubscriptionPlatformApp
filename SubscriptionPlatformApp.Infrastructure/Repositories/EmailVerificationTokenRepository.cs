using Microsoft.EntityFrameworkCore;
using SubscriptionPlatformApp.Application.Abstractions.Repositories;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Infrastructure.Persistence;
using SubscriptionPlatformApp.Infrastructure.Repositories.Shared;

namespace SubscriptionPlatformApp.Infrastructure.Repositories
{
    public class EmailVerificationTokenRepository : GenericRepository<EmailVerificationTokens>, IEmailVerificationTokenRepository
    {
        public EmailVerificationTokenRepository(AppDbContext db) : base(db) { }

        public Task<EmailVerificationTokens?> FindByIdAsync(Guid id, CancellationToken ct)
        {
            return _set
                .Include(e => e.User)
                .FirstOrDefaultAsync(x => x.EmailVerificationTokenId == id, ct);
        }
    }
}
