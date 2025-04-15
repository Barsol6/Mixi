using mixi.Components;
using mixi.Modules.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using mixi.Modules.Account;
using mixi.Modules.Generators;
using mixi.Modules.Generators.CharacterNameGenerator;
using mixi.Modules.UI;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddSingleton<ICharacterNameGenerator, CharacterNameGenerator>();
builder.Services.AddSingleton<CharacterNameGenerator, CharacterNameGenerator>();
builder.Services.AddScoped<PasswordHash>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<SignUp>();
builder.Services.AddSingleton<SignUpPopup,SignUpPopup>();
builder.Services.AddSingleton<MenuPopup,MenuPopup>();
builder.Services.AddSingleton<NameGeneratorPopup,NameGeneratorPopup>();
builder.Services.AddSingleton<PdfPopup,PdfPopup>();




builder
    .Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddDbContext<MixiDbContext>(options =>
{
    options.UseSqlite("Data Source = mixi.db");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddRouting();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}



app.UseAuthorization();

app.UseRouting();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStaticFiles();



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MixiDbContext>();
    db.Database.Migrate();
}

app.Run();

