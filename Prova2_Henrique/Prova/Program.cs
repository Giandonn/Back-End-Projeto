using Microsoft.EntityFrameworkCore;
using Prova.Data;
using Prova.Repositories;
using Prova.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MeuPolicy", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5501") // Ajuste a URL conforme necess�rio
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Registro dos reposit�rios e servi�os com inje��o de depend�ncias
builder.Services.AddScoped<MarcaRepository>();
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<MarcaService>();
builder.Services.AddScoped<ProdutoService>();

// Configura��o do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionando suporte a controladores e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura��o de ambiente (desenvolvimento)
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

// Habilitando o CORS para a API
app.UseCors("MeuPolicy");

// Configura��o de autentica��o/autoriza��es (caso necess�rio)
app.UseAuthorization();

// Mapeamento de controladores
app.MapControllers();

// Rodando a aplica��o
app.Run();
