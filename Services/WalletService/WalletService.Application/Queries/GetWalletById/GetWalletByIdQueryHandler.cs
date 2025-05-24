using Finvia.Shared.Common;
using MapsterMapper;
using MediatR;
using WalletService.Application.DTOs;
using WalletService.Domain.Abstractions;

namespace WalletService.Application.Queries.GetWalletById;

public class GetWalletByIdQueryHandler
    (IWalletRepository walletRepository, IMapper mapper) : IRequestHandler<GetWalletByIdQuery, Result<WalletDto>>
{
    public async Task<Result<WalletDto>> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByIdAsync(request.WalletId);

        return wallet is null
            ? Result<WalletDto>.Failure("Wallet not found")
            : Result<WalletDto>.Success(mapper.Map<WalletDto>(wallet));
    }
}