using Microsoft.EntityFrameworkCore;
using milescarrental.Infrastructure.Processing.InternalCommands;
using milescarrental.Infrastructure.Processing.Outbox;

namespace milescarrental.Infrastructure.Database
{
    public class OrdersContext : DbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<InternalCommand> InternalCommands { get; set; }
      
        public OrdersContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersContext).Assembly);
        }
    }
}
