using MassTransit;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;
using Domain.Services;
using Shared.Application.Contracts.Requests.Room;
using Shared.Application.Contracts.Requests.User;

namespace Application.CQRS.Roulette.Commands;

public sealed class PlaceRouletteBetCommandHandler(IRouletteService rouletteService, IRequestClient<ValidateAccessTokenRequest> accessTokenRequestClient, IRequestClient<ValidateUserRoomRequest> userRoomRequestClient) : ICommandHandler<PlaceRouletteBetCommand>
{
    public async Task<Result> Handle(PlaceRouletteBetCommand request, CancellationToken cancellationToken)
    {
        Response<ValidateAccessTokenResponse> validateTokenResponse = await accessTokenRequestClient.GetResponse<ValidateAccessTokenResponse>(new ValidateAccessTokenRequest
        {
            Issuer = request.Issuer,
            Subject = request.Subject
        }, cancellationToken);
        
        if (!validateTokenResponse.Message.IsValid)
            return Result.Failure(Error.ConditionNotMet);
        
        Response<ValidateUserRoomResponse> validateRoomResponse = await userRoomRequestClient.GetResponse<ValidateUserRoomResponse>(new ValidateUserRoomRequest
        {
            UserId = validateTokenResponse.Message.UserId!.Value,
            RoomId = request.RoomId
        });
        
        if (!validateRoomResponse.Message.IsValid)
            return Result.Failure(Error.ConditionNotMet);
        
        if (!rouletteService.ValidateBet(request.BetType, request.BetValues))
            return Result.Failure(Error.ConditionNotMet);
        
        return Result.Success();
    }
}