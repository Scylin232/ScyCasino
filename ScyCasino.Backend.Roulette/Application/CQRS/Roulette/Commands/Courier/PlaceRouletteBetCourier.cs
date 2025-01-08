using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransit.Initializers;
using Shared.Application.Contracts.Requests.Room;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Application.Contracts.Requests.User;
using Shared.Application.Events.Roulette;

namespace Application.CQRS.Roulette.Commands.Courier;

public class PlaceRouletteBetRequestProxy : RoutingSlipRequestProxy<PlaceRouletteBetCommand>
{
    protected override Task BuildRoutingSlip(RoutingSlipBuilder builder, ConsumeContext<PlaceRouletteBetCommand> context)
    {
        builder.AddActivity("ValidateUserAccessTokenActivity", new Uri("queue:validate-access-token_execute"), new ValidateAccessTokenContract
        {
            Subject = context.Message.Subject,
            Issuer = context.Message.Issuer
        });
        
        builder.AddActivity("ValidateUserRoomActivity", new Uri("queue:validate-user-room_execute"), new ValidateUserRoomContract
        {
            RoomId = context.Message.RoomId
        });
        
        builder.AddActivity("ConsumeCoinsActivity", new Uri("queue:consume-coins_execute"), new ConsumeCoinsContract
        {
            Amount = context.Message.Amount
        });
        
        builder.AddActivity("CreateRouletteBetActivity", new Uri("queue:create-roulette-bet_execute"), new CreateRouletteBetContract
        {
            BetType = (int)context.Message.BetType,
            BetValues = context.Message.BetValues
        });
        
        return Task.CompletedTask;
    }
}

public class PlaceRouletteBetResponseProxy : RoutingSlipResponseProxy<PlaceRouletteBetCommand, RouletteBetCreatedEvent>
{
    protected override async Task<RouletteBetCreatedEvent> CreateResponseMessage(ConsumeContext<RoutingSlipCompleted> context, PlaceRouletteBetCommand request)
    {
        SendTuple<RouletteBetCreatedEvent> response = await MessageInitializerCache<RouletteBetCreatedEvent>.InitializeMessage(new RouletteBetCreatedEvent
        {
            BetId = context.GetVariable<Guid>("BetId")!.Value,
            RoomId = request.RoomId
        });
        
        return response.Message;
    }
}

public class PlaceRouletteBetResponseProxyDefinition : ConsumerDefinition<PlaceRouletteBetResponseProxy>
{
    public PlaceRouletteBetResponseProxyDefinition(IEndpointNameFormatter endpointNameFormatter)
    {
        EndpointName = endpointNameFormatter.Consumer<PlaceRouletteBetRequestProxy>();
    }
}