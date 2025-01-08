using MassTransit;
using MassTransit.Courier.Contracts;
using Shared.Application.Abstractions.Messaging;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Application.Contracts.Requests.User;
using Shared.Kernel.Core;

namespace Application.CQRS.Roulette.Commands;

public sealed class CollectRouletteBetsCommandHandler(IBus bus) : ICommandHandler<CollectRouletteBetsCommand>
{
    public async Task<Result> Handle(CollectRouletteBetsCommand request, CancellationToken cancellationToken)
    {
        Random random = new Random();
        RoutingSlipBuilder routingSlipBuilder = new(NewId.NextGuid());
        
        routingSlipBuilder.AddActivity("CollectRouletteBetsActivity", new Uri("queue:collect-roulette-bets_execute"), new CollectRouletteBetsContract
        {
            WinningNumber = random.Next(0, 37)
        });
        routingSlipBuilder.AddActivity("MultiAddCoinsActivity", new Uri("queue:multi-add-coins_execute"));
        routingSlipBuilder.AddActivity("ClearRouletteGameStatesActivity", new Uri("queue:clear-roulette-game-states_execute"));
        
        RoutingSlip routingSlip = routingSlipBuilder.Build();
        
        await bus.Execute(routingSlip, cancellationToken);
        
        return Result.Success();
    }
}