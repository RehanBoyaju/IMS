using IMS.WebUI.Components;
using IMS.Infrastructure.EntityFramework;
using IMS.Infrastructure.EntityFramework.DependencyInjection;
using IMS.Application.DependencyInjection;
using IMS.WebUI.Components.Layout.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using IMS.WebUI.Hubs;
using IMS.WebUI.States;
using IMS.WebUI.States.Administration;
using IMS.WebUI.States.User;
using NetcodeHub.Packages.Extensions.LocalStorage;
using Syncfusion.Blazor;
using MudBlazor.Services;
using NetcodeHub.Packages.Components.DataGrid;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 

builder.Services.AddRazorComponents().AddInteractiveServerComponents(); ;
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin")); // Replace with your actual claim
});
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddScoped<AuthenticationStateProvider,IdentityRevalidatingAuthStateProvider>();
builder.Services.AddScoped<HubConnectionService>();
builder.Services.AddScoped<ICustomAuthorizationService, CustomAuthorizationService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ChangePasswordState>();
builder.Services.AddScoped<AdminActiveOrderCountState>();
builder.Services.AddScoped<GenericHomeHeaderState>();
builder.Services.AddScoped<UserActiveOrderCountState>();
builder.Services.AddVirtualizationService();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF1cXmhKYVFwWmFZfVtgcV9EZVZRRWYuP1ZhSXxWdkZiUH9bcnFVR2VcV0A=");
builder.Services.AddNetcodeHubLocalStorageService();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddMudServices();

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
app.MapHub<CommunicationHub>("/communicationHub");

app.Run();






//using IMS.Application.Inventories;
//using IMS.Application.Inventories.Interfaces;
//builder.Services.AddSingleton<InventoryRepository, InventoryRepository>();
//builder.Services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();