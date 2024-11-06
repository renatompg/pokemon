
using Microsoft.EntityFrameworkCore;
using PokemonApi.Database;
using PokemonApi.ExternalApi;
using PokemonApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddHttpClient<IPokemonService, PokemonService>();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Aqui você pode definir o comportamento desejado (por padrão, será PascalCase)
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=pokemon.db"));
builder.Services.AddHttpClient<IPokemonApiClient, PokemonApiClient>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<IPokemonMasterService, PokemonMasterService>();
builder.Services.AddScoped<IPokemonMasterRepository, PokemonMasterRepository>();
builder.Services.AddScoped<ICaptureRepository, CaptureRepository>();
// Configuração para controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
