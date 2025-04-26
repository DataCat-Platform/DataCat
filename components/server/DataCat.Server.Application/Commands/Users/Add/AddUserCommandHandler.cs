namespace DataCat.Server.Application.Commands.Users.Add;

// public sealed class AddUserCommandHandler(
//     IRepository<UserEntity, Guid> userRepository)
//     : IRequestHandler<AddUserCommand, Result<Guid>>
// {
//     public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
//     {
//         var id = Guid.NewGuid();
//         
//         var userResult = UserEntity.Create(id);
//         await userRepository.AddAsync(userResult.Value, cancellationToken); 
//
//         return Result.Success(id);
//     }
// }