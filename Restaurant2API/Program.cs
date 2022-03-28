using Restaurant2API;
using Restaurant2API.Entities;
using Restaurant2API.Services;
using NLog.Web;
using Restaurant2API.Middleware;
using Microsoft.AspNetCore.Identity;
using Restaurant2API.Models;
using FluentValidation;
using Restaurant2API.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Restaurant2API.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

/// Dodatkowe za³¹czenie "appsettings.json" do zmiennej configuration
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
///

//Authentication
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg=>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
        };
});


// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();
//builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddScoped<RestaurantSeed>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IValidator<RestaurantSearchQuery>, RestaurantSearchQueryValidation>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();

builder.Host.UseNLog();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

builder.Services.AddSwaggerGen();


//Cors  policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder =>

         builder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(configuration["AllowedOrigins"])
         );
});
//

//Dodatkowa autoryzacja z tokenu Jwt
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "Polish"));
    options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
    options.AddPolicy("MultiOwnerRequirement", builder => builder.AddRequirements(new CreatingMultipleRestourantRequirement(2)));
});
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CreatingMultipleRestourantRequirementHandler>();
//

var app = builder.Build();

using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
{
    var seeder = serviceProvider.GetRequiredService<RestaurantSeed>();
    seeder.Seed();
}

//Use cors
app.UseCors("FrontEndClient");

//MiddleWare i ErrorHandlinf
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

//Dodanie plików
app.UseStaticFiles();
//Casching
app.UseResponseCaching();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
