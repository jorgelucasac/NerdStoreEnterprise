using Estudos.NSE.Catalogo.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;

namespace Estudos.NSE.Catalogo.API.Data
{
    public class CatalogoDbContext : DbContext, IUnitOfWork
    {
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");

            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}