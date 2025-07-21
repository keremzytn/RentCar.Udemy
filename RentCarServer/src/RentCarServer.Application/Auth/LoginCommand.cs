using FluentValidation;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password) : IRequest<Result<string>>;


public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserNameOrEmail).NotEmpty().WithMessage("Geçerli bir mail yada kullanıcı adı girin");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Geçerli bir şifre girin");
    }
}

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
        p.Email.Value == request.UserNameOrEmail
        || p.UserName.Value == request.UserNameOrEmail);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı adı yada şifre yanlış");
        }
        var checkPassword = user.VerifyPasswordHash(request.Password);

        if (!checkPassword)
        {
            return Result<string>.Failure("Kullanıcı adı yada şifre yanlış");
        }

        var token = jwtProvider.CreateToken(user);

        return token;
    }
}