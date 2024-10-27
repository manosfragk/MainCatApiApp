using CatApiApp.Data;
using CatApiApp.Interfaces;
using CatApiApp.Repositories;
using CatApiApp.Services;
using Microsoft.EntityFrameworkCore;
using Refit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add Redis service
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "CatApiApp_";
});

// Add Swagger service configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Cat API",
        Description = "API to fetch and store cat images from an external API and manage related data",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Manos Fragkiadis",
            Email = "manolisfragiadis@gmail.com"
        }
    });

    // Add the XML comment documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Set database
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Refit to use the API key for all requests
builder.Services.AddRefitClient<ICatApiClient>()
    .ConfigureHttpClient(c =>
    {
        // Read CatApi settings from configuration
        var catApiSettings = builder.Configuration.GetSection("CatApi");
        var apiKey = catApiSettings["ApiKey"];
        var baseUrl = catApiSettings["BaseUrl"];

        c.BaseAddress = new Uri(baseUrl);
        c.DefaultRequestHeaders.Add("x-api-key", apiKey);
    });

// Register Repositories
builder.Services.AddScoped<ICatRepository, CatRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// Register Services
builder.Services.AddScoped<ICatService, CatService>();



// Ensure Kestrel is listening on port 80
if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        // Docker
        serverOptions.ListenAnyIP(80);
    });
}

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();