using Microsoft.Extensions.DependencyInjection;
using myBookSolution.Application.Services.AutoMapper;
using myBookSolution.Application.Services.CpfValidator;
using myBookSolution.Application.Services.Cryptography;
using myBookSolution.Application.UseCases.Author.AddAuthor;
using myBookSolution.Application.UseCases.Author.SearchAuthorByName;
using myBookSolution.Application.UseCases.Book.AddBook;
using myBookSolution.Application.UseCases.Book.SearchBookByName;
using myBookSolution.Application.UseCases.Borrowing.AddBorrowing;
using myBookSolution.Application.UseCases.Borrowing.Devolution;
using myBookSolution.Application.UseCases.Borrowing.GetUserBorrowing;
using myBookSolution.Application.UseCases.Curator.Login;
using myBookSolution.Application.UseCases.Curator.Profile;
using myBookSolution.Application.UseCases.Curator.Register;
using myBookSolution.Application.UseCases.Token.RefreshToken;
using myBookSolution.Application.UseCases.User.Login;
using myBookSolution.Application.UseCases.User.Profile;
using myBookSolution.Application.UseCases.User.Register;

namespace myBookSolution.Application;

public static class DependencyInjectionApplication
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddPasswordEncrypter(services);
        AddAutoMapper(services);
        AddUseCases(services);
        AddCpfValidator(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        //user
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUserRefreshTokenUseCase, UserRefreshTokenUseCase>();

        //curator
        services.AddScoped<IRegisterCuratorUseCase, RegisterCuratorUseCase>();
        services.AddScoped<IGetCuratorProfileUseCase, GetCuratorProfileUseCase>();
        services.AddScoped<ILoginCuratorUseCase, LoginCuratorUseCase>();
        services.AddScoped<ICuratorRefreshTokenUseCase, CuratorRefreshTokenUseCase>();
        
        //author
        services.AddScoped<IAddAuthorUseCase, AddAuthorUseCase>();
        services.AddScoped<ISearchAuthorByNameUseCase, SearchAuthorByNameUseCase>();

        //book
        services.AddScoped<IAddBookUseCase, AddBookUseCase>();
        services.AddScoped<ISearchBookByNameUseCase, SearchBookByNameUseCase>();

       //borrowing
        services.AddScoped<IAddBorrowingUseCase, AddBorrowingUseCase>();
        services.AddScoped<IDevolutionBookUseCase, DevolutionUseCase>();
        services.AddScoped<IUserGetBorrowingHistoryUseCase, UserGetBorrowingHistoryUseCase>();
    }

    private static void AddPasswordEncrypter(IServiceCollection services)
    {
        services.AddScoped(options => new PasswordEncrypter());
    }

    private static void AddCpfValidator(IServiceCollection services)
    {
        services.AddScoped<ICpfValidator, CpfValidator>();
    }
}
