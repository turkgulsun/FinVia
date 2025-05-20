using Finvia.Shared.Common;
using MapsterMapper;
using MediatR;
using WalletService.Application.Abstractions;
using WalletService.Application.DTOs;

namespace WalletService.Application.Queries.GetWalletById;

public class GetWalletByIdQueryHandler
    (IWalletService walletService, IMapper mapper) : IRequestHandler<GetWalletByIdQuery, Result<WalletDto>>
{
    public async Task<Result<WalletDto>> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var wallet = await walletService.GetByIdAsync(request.WalletId);

        return wallet is null
            ? Result<WalletDto>.Failure("Wallet not found")
            : Result<WalletDto>.Success(mapper.Map<WalletDto>(wallet));
    }
}