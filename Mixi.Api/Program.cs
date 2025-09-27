using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Account;
using Mixi.Api.Modules.Database;
using Mixi.Api.Modules.Database.Repositories.PdfRepositories;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.Generators.CharacterNameGenerator;
using Mixi.Api.Modules.Pdf;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
builder.Services.AddScoped<SignUp>();
builder.Services.AddSingleton<ICharacterNameGenerator, CharacterNameGenerator>();
builder.Services.AddSingleton<CharacterNameGenerator, CharacterNameGenerator>();


builder.Services.AddDbContext<MixiDbContext>(options =>
{
    options.UseSqlite("Data Source = mixi.db");
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

app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<MixiDbContext>();
        db.Database.Migrate();
    }
}


app.Run();

