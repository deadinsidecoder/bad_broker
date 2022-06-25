using BadBroker.DataAccess.Psql.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadBroker.DataAccess.Psql
{
	public class ApplicationContext : DbContext
	{
		public DbSet<CurrencyPair> CurrencyPairs { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CurrencyPair>()
				.HasKey(x => new { x.Date, x.QuoteBase, x.QuoteCurrency });
		}
	}
}