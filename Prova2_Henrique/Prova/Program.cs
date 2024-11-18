using Microsoft.EntityFrameworkCore;
using Prova.Data;
using Prova.Repositories;
using Prova.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MeuPolicy", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5501") // Ajuste a URL conforme necessário
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Registro dos repositórios e serviços com injeção de dependências
builder.Services.AddScoped<MarcaRepository>();
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<MarcaService>();
builder.Services.AddScoped<ProdutoService>();

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionando suporte a controladores e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração de ambiente (desenvolvimento)
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

// Configuração de autenticação/autorizações (caso necessário)
app.UseAuthorization();

// Mapeamento de controladores
app.MapControllers();

// Rodando a aplicação
app.Run();
