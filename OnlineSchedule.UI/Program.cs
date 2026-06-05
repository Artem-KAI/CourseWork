using OnlineSchedule.UI.Components;
using UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("ScheduleAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7007/");
});

builder.Services.AddScoped<ApiAuthService>();
//builder.Services.AddScoped<ApiScheduleService>();
//builder.Services.AddScoped<ApiStatisticsService>();
//builder.Services.AddScoped<ApiUserService>();

builder.Services.AddScoped<UserSessionService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
