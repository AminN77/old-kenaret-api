using Contracts.LoggerManager;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Kenaret.Extensions;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
                {
                    configuration.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console()
                    .Enrich.WithProperty("Environment", Environment.MachineName)
                    .ReadFrom.Configuration(context.Configuration);
                });

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureEventHandlers();
builder.Services.ConfigureActionFilters();
builder.Services.AddFileService();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddOtpService();
builder.Services.AddAutomapper();
builder.Services.AddBusinessLogicServices();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureCors();
builder.Services.AddJwt();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureLoggerService();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();
app.UseCors("CorsPolicy");
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseHttpsRedirection();
//app.UseIpRateLimiting();
//app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
