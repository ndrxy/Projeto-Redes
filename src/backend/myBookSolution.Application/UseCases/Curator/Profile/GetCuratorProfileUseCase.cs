using AutoMapper;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Services.LoggedCurator;

namespace myBookSolution.Application.UseCases.Curator.Profile;

public class GetCuratorProfileUseCase : IGetCuratorProfileUseCase
{
    private readonly ILoggedCurator _loggedCurator;
    private readonly IMapper _mapper;

    public GetCuratorProfileUseCase(ILoggedCurator loggedCurator, IMapper mapper)
    {
        _loggedCurator = loggedCurator;
        _mapper = mapper;
    }

    public async Task<ResponseUserProfile> Execute()
    {
        var curator = await _loggedCurator.CuratorLogged();

        return _mapper.Map<ResponseUserProfile>(curator);
    }
}
