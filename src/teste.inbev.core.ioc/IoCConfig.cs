using Microsoft.Extensions.DependencyInjection;
using teste.inbev.core.application.Applications;
using teste.inbev.core.data.Repositories;
using teste.inbev.core.domain.Interface.Application;
using teste.inbev.core.domain.Interface.Repositories;
using teste.inbev.core.domain.Interface.Services;
using teste.inbev.core.service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.ioc
{
    public static class IoCConfig
    {
        public static IServiceCollection ConfigIoC(this IServiceCollection services)
        {
            services.IoCModulos();
            return services;
        }
        public static IServiceCollection IoCModulos(this IServiceCollection services)
        {
            services.AddTransient<IProdutoApplication, ProdutoApplication>();
            services.AddTransient<IProdutoService, ProdutoService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();

            services.AddTransient<IOrcamentoApplication, OrcamentoApplication>();
            services.AddTransient<IOrcamentoService, OrcamentoService>();
            services.AddTransient<IOrcamentoRepository, OrcamentoRepository>();

            services.AddTransient<IOrcamentoItemService, OrcamentoItemService>();
            services.AddTransient<IOrcamentoItemRepository, OrcamentoItemRepository>();

            return services;
        }
    }
}
