using Blazored.LocalStorage;
using Gym.Client.Components;
using Gym.Client.Interfaces;
using Gym.Client.Security;
using Gym.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://192.168.100.118:7120");
builder.Services.AddMudServices();
builder.Services.AddRazorPages();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

builder.Services.AddAuthorizationCore();




builder.Services.AddHttpClient("MainApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7136");
});
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MainApi"));


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();  
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
