using Engine;
using Engine.Services.StartupService;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Add();

    builder.Host.UseSerilog((ctx, services, lc) =>
        lc.ReadFrom.Configuration(ctx.Configuration)
          .Enrich.FromLogContext()
    );

    builder.Configuration.AddJsonFile("entityfieldtypes.json", optional: false, reloadOnChange: true);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.Logger.LogInformation("***********************************");
    app.Logger.LogInformation("*** Starting up MetaFlow Engine ***");
    app.Logger.LogInformation("***********************************");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var startupService = app.Services.GetRequiredService<IStartupService>();
        await startupService.InitApp();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
