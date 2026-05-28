using Futvibe.Application.Common.DTOs;
using Futvibe.Application.Common.Mappers;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepo)
    : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await userRepo.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException($"Usuário {request.UserId} não encontrado.");

        return UserMapper.ToDto(user);
    }
}
