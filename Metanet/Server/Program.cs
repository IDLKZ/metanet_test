using Metanet.Server.Database;
using Metanet.Server.Helpers;
using Metanet.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Toolbelt.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Json;
using System.Security.Claims;
using Metanet.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

var security = new Dictionary<string, IEnumerable<string>>{{"Bearer", new string[] { }},};

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c=>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "����������, ������� �����",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        }
      );
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        }
        );


    });
// Add services to the container.
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
//Using Mysql As DATABASE
builder.Services.AddDbContext<ApplicationDbContext>(
    options=>options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
//Add Identity to our Project
builder.Services.AddIdentity<ApplicationUser,Role>(opts =>
{
    opts.User.AllowedUserNameCharacters = null;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

//mail settings
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();
//Add Scoped
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

//http and jwt
builder.Services.AddHttpClient();
var apiSettings = builder.Configuration.GetSection("APISettings");
builder.Services.Configure<APISettings>(apiSettings);
var apiSet = apiSettings.Get<APISettings>();
var key = Encoding.ASCII.GetBytes(apiSet.SecretKey);
Microsoft.AspNetCore.Authentication.AuthenticationBuilder authenticationBuilder = builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidAudience = apiSet.ValidAudience,
        ValidIssuer = apiSet.ValidIssuer,
        ClockSkew = TimeSpan.Zero

    };
});
//end of jwt and auth

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => 
        builder.WithOrigins("https://localhost:5001", "https://ecom.jysanbank.kz/ecom/api")
            .AllowAnyMethod()
            .AllowAnyHeader());
}); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCssLiveReload();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//Swagger
//using static files for uploads
app.UseStaticFiles();                     // leave this one in
app.UseStaticFiles(new StaticFileOptions
{
  FileProvider = new PhysicalFileProvider(
  System.IO.Path.Combine(builder.Environment.ContentRootPath, "uploads" )),  
  RequestPath = "/uploads"
});

//End Swagger
// app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyOrigin());
app.Run();
