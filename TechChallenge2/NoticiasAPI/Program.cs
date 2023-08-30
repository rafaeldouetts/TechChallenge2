using TechChallenge.NoticiasAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TechChallenge.Identity.Controllers;
using TechChallenge.Identity.Extensions;
using NoticiasAPI.Controllers;
using NoticiasAPI.Service;
using ElmahCore.Mvc;
using ElmahCore;

var builder = WebApplication.CreateBuilder(args);

//AddDbContext
builder.Services.AddDbContext<NoticiasContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AppNoticiasConnection")));

builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


//HttpClient
builder.Services.AddHttpClient<AuthenticateController>();
builder.Services.AddHttpClient<NoticiaController>();

builder.Services.AddDIAuthentication(builder);

builder.Services.AddDICors(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Acesso protegido utilizando o accessToken obtido em \"api/Authenticate/login\""
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});
builder.Services.AddScoped<INoticiaService, NoticiaService>();
builder.Services.AddElmah<XmlFileErrorLog>(options =>
{
    options.LogPath = "~/log";
});
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

//cors
app.UseCors("CorsPolicy-public");

app.MapControllers();
app.UseElmah();
app.Run();