using AsyncProgrammingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Dodaj serwisy
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rejestracja naszego serwisu
builder.Services.AddScoped<IAsyncDemonstrationService, AsyncDemonstrationService>();
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Jeśli potrzebne
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactApp");
// app.UseHttpsRedirection();
app.UseRouting();              // Po CORS i HTTPS
app.UseAuthorization();
app.MapControllers();

app.Run();
