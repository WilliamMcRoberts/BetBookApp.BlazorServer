using BetBookUI;
using Microsoft.AspNetCore.Rewrite;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application Starting...");
    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices();

    var app = builder.Build();

    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
        builder.Configuration.GetValue<string>("Syncfusion:Key"));

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    // Redirect
    app.UseRewriter(
        new RewriteOptions().Add(
            context =>
            {
                if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
                {
                    context.HttpContext.Response.Redirect("/");
                }
            }
            ));

    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.OrdinalIgnoreCase)) throw;
    Log.Fatal(ex, "The Host Stopped Unexpectedly...");
}
finally
{
    Log.CloseAndFlush();
}
