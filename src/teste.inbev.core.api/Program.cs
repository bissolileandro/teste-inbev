using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using teste.inbev.core.application;
using teste.inbev.core.application.Configurations;
using teste.inbev.core.application.Validators;
using teste.inbev.core.data.Context;
using teste.inbev.core.ioc;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddValidatorsFromAssemblyContaining<ProdutoInserirValidator>();

var connectionString = builder.Configuration.GetConnectionString("InbevCoreContext");
builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<InbevCoreContext>(options =>
    {        
        options.UseSqlServer(connectionString,sqlOptions => sqlOptions.MigrationsAssembly("teste.inbev.core.data"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });

builder.Services.ConfigIoC();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
