using System.Threading.RateLimiting;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using RentCarServer.Application;
using RentCarServer.Infrastructure;
using RentCarServer.WebAPI;
using Scalar.AspNetCore;
using RentCarServer.WebAPI.Modules;
using RentCarServer.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.EnvironmentName = "Development";

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("login-fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.QueueLimit = 1;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
builder.Services
            .AddControllers()
            .AddOData(opt =>
            opt.Select()
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .SetMaxTop(null)
            );
builder.Services.AddCors();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();
builder.Services.AddResponseCompression(
    options =>
    {
        options.EnableForHttps = true;
    }
);

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseCors(x => x
.AllowAnyHeader()
.AllowAnyOrigin()
.AllowAnyMethod()
.SetPreflightMaxAge(TimeSpan.FromMinutes(10)));

app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.UseExceptionHandler();

app.MapControllers()
.RequireRateLimiting("fixed")
.RequireAuthorization();

app.MapAuthModule();

app.MapGet("/", () => "Hello World").RequireAuthorization();
//await app.CreateFirstUser();
app.Run();