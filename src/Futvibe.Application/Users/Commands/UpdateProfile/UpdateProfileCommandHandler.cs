using Futvibe.Application.Common.DTOs;
using Futvibe.Application.Common.Mappers;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler(IUserRepository userRepo)
    : IRequestHandler<UpdateProfileCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var user = await userRepo.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException($"Usuário {request.UserId} não encontrado.");

        user.UpdateProfile(request.Name, request.Bio, request.Level);

        await userRepo.SaveChangesAsync(ct);

        return UserMapper.ToDto(user);
    }
}
