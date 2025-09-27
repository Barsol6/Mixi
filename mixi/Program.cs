using mixi.Components;
using mixi.Modules.Database;
using Microsoft.EntityFrameworkCore;
using mixi.Modules.Pdf;
using mixi.Modules.Account;
using mixi.Modules.Generators.CharacterNameGenerator;
using mixi.Modules.UI;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();










builder
    .Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddRouting();




var app = builder.Build();

// Configure the HTTP request pipeline.




app.UseAuthorization();

app.UseRouting();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStaticFiles();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<MixiDbContext>();
        db.Database.Migrate();
    }
}


app.Run();

