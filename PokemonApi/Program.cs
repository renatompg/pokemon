
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PokemonApi.Database;
using PokemonApi.ExternalApi;
using PokemonApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IPokemonService, PokemonService>();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=pokemon.db"));
builder.Services.AddHttpClient<IPokemonApiClient, PokemonApiClientV2>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<IPokemonMasterService, PokemonMasterService>();
builder.Services.AddScoped<IPokemonMasterRepository, PokemonMasterRepository>();
builder.Services.AddScoped<ICaptureRepository, CaptureRepository>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokemon API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
