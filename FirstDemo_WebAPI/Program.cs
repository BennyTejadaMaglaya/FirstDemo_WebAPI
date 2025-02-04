using FirstDemo_WebAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MedicalOfficeContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MedicalOfficeContext")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

//To prepare the database and seed data.  Can comment this out some of the time.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    MedicalOfficeInitializer.Initialize(serviceProvider: services, DeleteDatabase: true,
        UseMigrations: true, SeedSampleData: true);
}

app.Run();
