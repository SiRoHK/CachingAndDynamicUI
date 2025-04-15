using CachingAndDynamicUI.Interfaces;
using CachingAndDynamicUI.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));


//builder.Services.AddSingleton<ICacheService, RedisCacheService>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, HybridCacheService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IApiService, JsonPlaceholderApiService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();