using Shared.Application.Abstractions.Messaging;
using Domain.Repositories;
using Shared.Kernel.Core;
using Shared.Kernel.Repositories;

namespace Application.CQRS.Room.Commands;

public sealed class DeleteRoomByIdCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteRoomByIdCommand>
{
    public async Task<Result> Handle(DeleteRoomByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.Room? room = await roomRepository.GetById(request.Id);
        
        if (room is null)
            return Result.Failure(Error.NotFound);
        
        await roomRepository.Remove(room);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}