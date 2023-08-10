using NCRFiscalManager.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NCRFiscalManager.Core.Interfaces.Facture;
using NCRFiscalManager.Core.Interfaces.GoSocket;
using NCRFiscalManager.Infraestructure.ThirdParty.DBF.Implementation;
using NCRFiscalManager.Infraestructure.ThirdParty.Facture.Services;
using NCRFiscalManager.Infraestructure.ThirdParty.GoSocket.Services;
using NCRFiscalManager.Infraestructure.ThirdParty.Tecnofactor.Services;
using Microsoft.Extensions.Configuration;
using NCRFiscalManager.Infraestructure.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Infraestructure.Repositories;
using NCRFiscalManager.Infraestructure.ThirdParty.DataLayer.Services;

namespace NCRFiscalManager.Infraestructure
{
    public static class ServiceExtensions
    {
        private const string _fiscalConnectionString = "NcrFiscalDatabase";
        public static IServiceCollection AddInfraestructureServices(this IServiceCollection services)
        {
            services.addDataBaseContext();

            services.AddScoped<ISendDocumentTecnoFactorService, SendDocumentTecnofactorService>();
            services.AddScoped<ISendDocumentGoSocketService, SendDocumentGoSocketService>();
            services.AddScoped<ISendDocumentFactureService, SendDocumentFactureService>();
            services.AddScoped<IDBFManager, DBFManager>();

            services.AddScoped<IBasicAuthUserRepository, BasicAuthUserRepository>();
            services.AddScoped<IInvoiceTransactionRepository, InvoiceTransactionRepository>();
            services.AddScoped<IPaymentMethodsRepository, PaymentMethodsRepository>();
            services.AddScoped<IPointOnSaleRepository, PointOnSaleRepository>();
            services.AddScoped<ITechOperatorRepository, TechOperatorRepository>();
            services.AddScoped<ITechOperatorEmitterInVoiceRepository, TechOperatorEmitterInVoiceRepository>();
            services.AddScoped<IEmitterInVoiceRepository, EmitterInVoiceRepository>();
            services.AddScoped<IBlackListItemsRepository, BlackListItemsRepository>();

            services.AddScoped<IInvoiceTransactionPersistenceService, InvoiceTransactionPersistenceService>();
            services.AddScoped<IPaymentMethodsService, PaymentMethodService>();


            return services;
        }

        public static IServiceCollection addDataBaseContext(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddDbContext<NCRFiscalContext>(optionsAction: options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(_fiscalConnectionString));
            });
            return services;
        }
    }
}
