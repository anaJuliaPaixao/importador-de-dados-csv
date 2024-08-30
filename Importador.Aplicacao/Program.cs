using Microsoft.EntityFrameworkCore;
using Importador.Servico.Servicos;
using Importador.Dominio.Interfaces.Servicos;
using Importador.Dominio.Interfaces.Repositorios;
using Importador.Infra.Repositorios;
using Importador.Dominio.Entidades;
using Importador.Repositorio.Querys;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AplicacaoContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
        ServiceLifetime.Scoped);

builder.Services.AddScoped<IImportadorServico, ImportadorServicos>();
builder.Services.AddScoped<INotificacaoServico, NotificacaoServico>();
builder.Services.AddScoped<ICatalagoRepositorio, CatalagoRepositorio>();
builder.Services.AddScoped<ICatalagoColunaRepositorio, CatalagoColunaRepositorio>();
builder.Services.AddScoped<IDadosCatalagoRepositorio, DadosCatalagosRepositorio>();

var app = builder.Build();

// Configurar o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();