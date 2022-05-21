using Blazored.LocalStorage;
using Metanet.Client;
using Metanet.Client.Services;
using Metanet.Client.Services.Auth;
using Metanet.Client.Services.FileService;
using Metanet.Client.Services.IServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sotsera.Blazor.Toaster.Core.Models;
using Toolbelt.Extensions.DependencyInjection; // <- Add this, and...

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddToaster(config =>
{
    //example customizations
    config.PositionClass = Defaults.Classes.Position.TopRight;
    config.PreventDuplicates = true;
    config.NewestOnTop = false;
});
//Authentication To Client Side
builder.Services.AddScoped<AuthenticationStateProvider, AuthState>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IFileUploader, FileUploader>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
