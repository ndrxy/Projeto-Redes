using AutoMapper;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;

namespace myBookSolution.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUser, Domain.Models.UserModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestRegisterUser, Domain.Models.CuratorModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestAddAuthor, AuthorModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CuratorId, opt => opt.Ignore());

        CreateMap<RequestAddBook, BookModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CuratorId, opt => opt.Ignore());

        CreateMap<RequestAddBorrowing, BorrowingModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Models.UserModel, ResponseUserProfile>();
        CreateMap<CuratorModel, ResponseUserProfile>();
    }

}
