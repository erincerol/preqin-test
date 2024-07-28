using InvestorsApi.Data;
using InvestorsApi.Repositories;
using InvestorsApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<InvestorsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("InvestorsDatabase")));

builder.Services.AddScoped<IInvestorRepository, InvestorRepository>();
builder.Services.AddScoped<ICommitmentRepository, CommitmentRepository>();
builder.Services.AddScoped<IInvestorService, InvestorService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<InvestorsContext>();
    DbInitializer.Initialize(context);
}

app.Run();
