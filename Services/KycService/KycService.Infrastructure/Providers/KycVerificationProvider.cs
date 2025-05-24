using Finvia.Shared.Application.Enums;
using KycService.Application.Abstractions;

namespace KycService.Infrastructure.Providers;

public class KycVerificationProvider : IKycVerificationProvider
{
    public Task<KycVerificationResult> VerifyAsync(Guid userId)
    {
        // Demo
        var random = new Random();
        var values = Enum.GetValues(typeof(KycVerificationResult));
        var result = (KycVerificationResult)values.GetValue(random.Next(values.Length))!;

        return Task.FromResult(result);
    }
}