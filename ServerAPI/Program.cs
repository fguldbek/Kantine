using Core.Models;
using ServerAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddSingleton<IEventsRepository, EventsRepository>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<ILoginRepository, LoginRepository>();
builder.Services.AddSingleton<ICompanyRepository, CompanyRepository>();

// Swagger configuration for documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS policy in middleware (only need to call once)
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();