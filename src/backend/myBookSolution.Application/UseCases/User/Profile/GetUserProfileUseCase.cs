using AutoMapper;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Services.LoggedUser;

namespace myBookSolution.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseUserProfile> Execute()
    {
        var user = await _loggedUser.UserLogged();

        return _mapper.Map<ResponseUserProfile>(user);
    }
}
