using KycService.Domain.Entities;

namespace KycService.Domain.Abstractions;

public interface IKycRepository
{
    Task<Kyc?> GetByUserIdAsync(Guid userId);
    Task AddAsync(Kyc kyc);
    Task UpdateAsync(Kyc kyc);
}