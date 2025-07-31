using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record ForgotPasswordCommand(
    string Email) : IRequest<Result<string>>;

public sealed class ForgotPasswordCommandValidator :
AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Geçerli bir e-posta adresi giriniz.")
        .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");
    }
}

internal sealed class ForgotPasswordCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMailService mailService) : IRequestHandler<ForgotPasswordCommand,
    Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository
        .FirstOrDefaultAsync(p => p.Email.Value == request.Email, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        user.CreateForgotPasswordId();
        await unitofWork.SaveChangesAsync(cancellationToken);
         
        string to = user.Email.Value;
        string subject = "Şifre Sıfırlama";
        string body = @"";

        await mailService.SendAsync(to, subject, body, cancellationToken);

        //şifre sıfırlama maili gönder

        return "Şifre sıfırlama maili gönderildi.";

    }
}