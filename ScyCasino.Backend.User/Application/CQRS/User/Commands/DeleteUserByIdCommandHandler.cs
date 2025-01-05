using Application.Abstractions.Messaging;
using Domain.Repositories;
using SharedKernel.Core;
using SharedKernel.Repositories;

namespace Application.CQRS.User.Commands;

public sealed class DeleteUserByIdCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserByIdCommand>
{
    public async Task<Result> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.User? user = await userRepository.GetById(request.Id);
        
        if (user is null)
            return Result.Failure(Error.NotFound);
        
        await userRepository.Remove(user);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}