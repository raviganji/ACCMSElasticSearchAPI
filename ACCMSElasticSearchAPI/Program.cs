var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var allowedOrigins = new List<string>();
var origins = "http://localhost:4200";
if (origins != null)
{
    var additionalOrigins = origins.Split(",", StringSplitOptions.RemoveEmptyEntries);
    allowedOrigins.AddRange(additionalOrigins);
}
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", x =>
    {
        x.AllowAnyMethod().AllowAnyHeader()
            .WithOrigins(allowedOrigins.ToArray())
            .AllowCredentials();
    });
});


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
app.UseRouting();
app.UseCors("AllowAny");
app.UseAuthorization();

app.Run();
