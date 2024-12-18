using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web;
using DevSkill.Inventory.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Worker;
using Amazon.SQS;

#region Bootstrap Logger Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
#endregion

try
{
    Log.Information("Application Starting...");
    var builder = WebApplication.CreateBuilder(args);

    #region Serilog General Configuration
    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.MSSqlServer(
            connectionString: configuration.GetConnectionString("DefaultConnection"),
            sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = false },
            columnOptions: new Serilog.Sinks.MSSqlServer.ColumnOptions()
        )
        .WriteTo.File(
            path: "Logs/InventoryWeb-log-.log",
            rollingInterval: RollingInterval.Day
        )
        .ReadFrom.Configuration(ctx.Configuration));
    #endregion

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    #region Autofac Configuration

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly));
    });

    #endregion

    #region Automapper Configuration
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
	#endregion

	// Register AWS services
	builder.Services.AddDefaultAWSOptions(configuration.GetAWSOptions());
	builder.Services.AddAWSService<IAmazonSQS>();

	// Register the ISQSService and its implementation
	builder.Services.AddScoped<ISQSService, SQSService>();

	builder.WebHost.UseUrls("http://*:80");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDbContext<InventoryDbContext>(options =>
       options.UseSqlServer(connectionString, (x)=> x.MigrationsAssembly(migrationAssembly)));
    
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddIdentity();
    builder.Services.AddControllersWithViews();

	builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
	
	var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
         name: "areas",
         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    //app.MapRazorPages();

    // Generate some test logs
    Log.Information("This is an information log entry.");
    Log.Warning("This is a warning log entry.");
    Log.Error("This is an error log entry.");
    Log.Fatal("This is a fatal log error.");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start application");
}
finally
{
    Log.CloseAndFlush();
}
