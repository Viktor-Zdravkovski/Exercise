using BasicWeb.Database.Context;
using BasicWeb.Helpers;
using BasicWeb.Mapper;
using BasicWeb.Shared.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var basicWebSettings = builder.Configuration.GetSection("BasicWebAppSettings");
builder.Services.Configure<BasicWebAppSettings>(basicWebSettings);
BasicWebAppSettings basicWebAppSettings = basicWebSettings.Get<BasicWebAppSettings>();

builder.Services.ConfigureSwagger();

builder.Services.AddDbContext<BasicWebDbContext>(options =>
    options.UseSqlServer(basicWebAppSettings.ConnectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile));

DependencyInjectionHelper.InjectRepositories(builder.Services);
DependencyInjectionHelper.InjectServices(builder.Services);

builder.Services.ConfigureAuthentication(basicWebAppSettings.SecretKey);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();


// auto migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<BasicWebDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.Run();
