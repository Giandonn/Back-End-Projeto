using Microsoft.EntityFrameworkCore;
using Prova.Data;
using Prova.Repositories;
using Prova.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MeuPolicy", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5502")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddScoped<MarcaRepository>();
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<MarcaService>();
builder.Services.AddScoped<ProdutoService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("MeuPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
