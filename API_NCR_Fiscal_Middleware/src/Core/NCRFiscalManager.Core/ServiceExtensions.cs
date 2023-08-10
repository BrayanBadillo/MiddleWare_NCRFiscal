using Serilog;
using AutoMapper;
using NCRFiscalManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Facture;
using NCRFiscalManager.Core.Interfaces.GoSocket;
using NCRFiscalManager.Core.Interfaces.Logging;
using NCRFiscalManager.Core.Interfaces.Services;
using NCRFiscalManager.Core.Mappers.Facture;
using NCRFiscalManager.Core.Mappers.GoSocket;
using NCRFiscalManager.Core.Mappers.TecnoFactor;
using NCRFiscalManager.Core.Services.Facture;
using NCRFiscalManager.Core.Services.Logging;
using NCRFiscalManager.Core.Mappers.Entities;

namespace NCRFiscalManager.Core
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddLoggingServices();
            services.AddSingleton(Log.Logger);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DetalleProfile());
                mc.AddProfile(new RequestDocumentoProfile());
                mc.AddProfile(new DetalleGoSocketProfile());
                mc.AddProfile(new DteProfile());
                mc.AddProfile(new FactureProfile());
                mc.AddProfile(new EmitterInvoiceMapperProfile());
                mc.AddProfile(new BlackListItemsMapperProfile());
                mc.AddProfile(new PointOfSalesMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddScoped<IBasicAuthUserService, BasicAuthUserService>();
            services.AddScoped<IDocumentManagerServices, DocumentManagerService>();
            services.AddScoped<IDocumentManagerTecnoFactorService, DocumentManagerTecnoFactorService>();
            services.AddScoped<IDocumentManagerGoSocketService, DocumentManagerGoSocketService>();
            services.AddScoped<IDocumentManagerFactureService, DocumentManagerFactureService>();
            services.AddScoped<IMapperGoSocketService, MapperGoSocketService>();
            services.AddScoped<IMapperTecnoFactorService, MapperTecnoFactorService>();
            services.AddScoped<IMapperFactureService, MapperFactureService>();
            services.AddScoped<IEmitterInvoiceService, EmitterInvoiceService>();
            services.AddScoped<IBlackListedItemsService, BlackListedItemsService>();
            services.AddScoped<IPointOfSalesService, PointOfSalesService>();

            return services;
        }
        
        private static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggingConfigurations, LoggingConfigurations>();

            ILoggingConfigurations loggingConfiguration = services.BuildServiceProvider()
                .GetService<ILoggingConfigurations>();

            loggingConfiguration.ConfigureLogs();

            return services;
        }
    }
}
