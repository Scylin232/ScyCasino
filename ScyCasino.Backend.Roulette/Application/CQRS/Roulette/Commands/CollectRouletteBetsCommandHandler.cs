using Domain.Services;
using MassTransit;
using MassTransit.Courier.Contracts;
using Shared.Application.Abstractions.Messaging;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Kernel.Core;

namespace Application.CQRS.Roulette.Commands;

public sealed class CollectRouletteBetsCommandHandler(IRouletteService rouletteService, IBus bus) : ICommandHandler<CollectRouletteBetsCommand>
{
    public async Task<Result> Handle(CollectRouletteBetsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            RoutingSlipBuilder routingSlipBuilder = new(NewId.NextGuid());
            
            routingSlipBuilder.AddActivity("CollectRouletteBetsActivity", new Uri("queue:collect-roulette-bets_execute"), new CollectRouletteBetsContract
            {
                WinningNumber = rouletteService.GetRandomWinningNumber()
            });
            routingSlipBuilder.AddActivity("MultiAddCoinsActivity", new Uri("queue:multi-add-coins_execute"));
            routingSlipBuilder.AddActivity("ClearRouletteGameStatesActivity", new Uri("queue:clear-roulette-game-states_execute"));
            
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