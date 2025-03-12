//using IMS.Application.Inventories;
//using IMS.Application.Inventories.Interfaces;
using IMS.WebUI.Components;
using IMS.Infrastructure.EntityFramework;
using IMS.Infrastructure.EntityFramework.DependencyInjection;
using IMS.Application.DependencyInjection;
using IMS.WebUI.Components.Layout.Identity;
using Microsoft.AspNetCore.Components.Authorization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.   b

builder.Services.AddRazorComponents().AddInteractiveServerComponents(); ;
//builder.Services.AddSingleton<InventoryRepository, InventoryRepository>();
//builder.Services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider,IdentityRevalidatingAuthStateProvider>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();   
app.UseAntiforgery();

app.MapStaticAssets();
app.MapSignOutEndpoint();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
