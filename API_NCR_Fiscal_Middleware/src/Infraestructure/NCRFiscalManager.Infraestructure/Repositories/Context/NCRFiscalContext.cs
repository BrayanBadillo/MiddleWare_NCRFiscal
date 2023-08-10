using Microsoft.EntityFrameworkCore;
using NCRFiscalManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NCRFiscalManager.Infraestructure.Repositories.Context
{
    public class NCRFiscalContext : DbContext
    {
        public NCRFiscalContext(DbContextOptions<NCRFiscalContext> options) : base(options) { }


        DbSet<BasicAuthUser> BasicAuthUsers { get; set; }
        public DbSet<TechOperator> TechOperators { get; set; }
        public DbSet<TechOperatorEmitterInVoice> TechOperatorEmitterInVoice { get; set; }
        public DbSet<EmitterInVoice> EmitterInVoice { get; set; }
        public DbSet<PointOnSale> PointOnSales { get; set; }
        public DbSet<PaymentMethods> PaymentMethods { get; set; }
        public DbSet<InvoiceTransaction> InvoiceTransactions { get; set; }
        public DbSet<BlackListItems> BlackListedItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
