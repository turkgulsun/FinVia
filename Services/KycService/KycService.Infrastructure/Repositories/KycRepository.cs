using KycService.Domain.Abstractions;
using KycService.Domain.Entities;
using KycService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KycService.Infrastructure.Repositories;

public class KycRepository(KycDbContext db) : IKycRepository
{
    public async Task<Kyc?> GetByUserIdAsync(Guid userId)
    {
        return await db.Kycs.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task AddAsync(Kyc kyc)
    {
        db.Kycs.Add(kyc);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Kyc kyc)
    {
        db.Kycs.Update(kyc);
        await db.SaveChangesAsync();
    }
}