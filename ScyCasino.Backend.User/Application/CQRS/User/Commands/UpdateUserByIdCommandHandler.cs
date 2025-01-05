using Application.Abstractions.Messaging;
using Domain.Repositories;
using SharedKernel.Core;
using SharedKernel.Repositories;

namespace Application.CQRS.User.Commands;

public sealed class UpdateUserByIdCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserByIdCommand>
{
    public async Task<Result> Handle(UpdateUserByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.User? user = await userRepository.GetById(request.Id);
        
        if (user is null)
            return Result.Failure(Error.NotFound);
        
        user.Coins = request.User.Coins;
        user.Nickname = request.User.Nickname;
        
        await userRepository.Update(user);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}