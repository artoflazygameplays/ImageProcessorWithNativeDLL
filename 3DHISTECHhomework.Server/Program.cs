using System.Text.Json.Serialization;
using _3DHISTECHhomework.Server.Service.Injection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Image Processing Backend",
        Version = "v1"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("angular",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

Inject.AddService(builder.Services);

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("angular");

app.UseAuthorization();

app.MapControllers();

app.Run();
