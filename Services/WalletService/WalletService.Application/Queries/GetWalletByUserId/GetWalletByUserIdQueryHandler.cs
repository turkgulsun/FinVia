using Finvia.Shared.Common;
using Mapster;
using MediatR;
using WalletService.Application.DTOs;
using WalletService.Domain.Abstractions;
using WalletService.Domain.Messages;

namespace WalletService.Application.Queries.GetWalletByUserId;

public class GetWalletByUserIdQueryHandler(IWalletRepository walletRepository) : IRequestHandler<GetWalletByUserIdQuery, Result<WalletDto>>
{
    public async Task<Result<WalletDto>> Handle(GetWalletByUserIdQuery query, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByUserIdAsync(query.UserId);

        if (wallet == null)
            return Result<WalletDto>.Failure(WalletMessages.NotFound);

        var dto = wallet.Adapt<WalletDto>();

        return Result<WalletDto>.Success(dto);
    }
}