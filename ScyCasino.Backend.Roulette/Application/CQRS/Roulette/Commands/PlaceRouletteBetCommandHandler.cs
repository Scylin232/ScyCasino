using MassTransit;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;
using Domain.Services;
using MassTransit.Courier.Contracts;
using Shared.Application.Contracts.Requests.Room;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Application.Contracts.Requests.User;

namespace Application.CQRS.Roulette.Commands;

public sealed class PlaceRouletteBetCommandHandler(IRouletteService rouletteService, IBus bus) : ICommandHandler<PlaceRouletteBetCommand>
{
    public async Task<Result> Handle(PlaceRouletteBetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!rouletteService.ValidateBet(request.BetType, request.BetValues))
                return Result.Failure(Error.ConditionNotMet);
            
            RoutingSlipBuilder routingSlipBuilder = new(NewId.NextGuid());
            
            routingSlipBuilder.AddActivity("ValidateUserAccessTokenActivity", new Uri("queue:validate-access-token_execute"), new ValidateAccessTokenContract
            {
                Subject = request.Subject,
                Issuer = request.Issuer
            });
            
            routingSlipBuilder.AddActivity("ValidateUserRoomActivity", new Uri("queue:validate-user-room_execute"), new ValidateUserRoomContract
            {
                RoomId = request.RoomId
            });
            
            routingSlipBuilder.AddActivity("ConsumeCoinsActivity", new Uri("queue:consume-coins_execute"), new ConsumeCoinsContract
            {
                Amount = request.Amount
            });
            
            routingSlipBuilder.AddActivity("CreateRouletteBetActivity", new Uri("queue:create-roulette-bet_execute"), new CreateRouletteBetContract
            {
                BetType = (int)request.BetType,
                BetValues = request.BetValues
            });
            
            routingSlipBuilder.AddActivity("UpdateRouletteGameStateActivity", new Uri("queue:update-roulette-game-state_execute"), new UpdateRouletteGameStateContract
            {
                RoomId = request.RoomId
            });
            
            RoutingSlip routingSlip = routingSlipBuilder.Build();
            await bus.Execute(routingSlip, cancellationToken);
            
            return Result.Success();
        }
        catch
        {
            return Result.Failure(Error.ConditionNotMet);
        }
    }
}