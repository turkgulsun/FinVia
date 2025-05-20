using Finvia.Shared.Common;
using MediatR;

namespace WalletService.Application.Commands.CreateWallet;

public record CreateWalletCommand(Guid UserId, string Currency) : IRequest<Result<Guid>>;