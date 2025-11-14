using System.Text;
using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Account;
using Mixi.Api.Modules.Database;
using Mixi.Api.Modules.Database.Repositories.NotesRepositories;
using Mixi.Api.Modules.Database.Repositories.PdfRepositories;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.Generators.CharacterNameGenerator;
using Mixi.Api.Modules.Pdf;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var config = builder.Configuration;
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
        };
    });

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 20 * 1024 * 1024;
});

builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Mixi.Api", Version = "v1" });
    }
);
builder.Services.AddControllers();
builder.Services.AddScoped<PasswordHash>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPdfRepository, PdfRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddScoped<SignUp>();
builder.Services.AddSingleton<ICharacterNameGenerator, CharacterNameGenerator>();
builder.Services.AddSingleton<CharacterNameGenerator, CharacterNameGenerator>();


builder.Services.AddDbContext<MSSQLMixiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection")));
builder.Services.AddSingleton<MongoMixiDbContext>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return new MongoMixiDbContext(client, "MixiDB");
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<MSSQLMixiDbContext>();
        db.Database.Migrate();
    }
    
}


app.Run();

