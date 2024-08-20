using Microsoft.EntityFrameworkCore;

namespace Frenet.ShipManagement.Data
{
    public class FrenetShipManagementContext : DbContext
    {
        public FrenetShipManagementContext(DbContextOptions<FrenetShipManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Frenet.ShipManagement.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<Frenet.ShipManagement.Models.Pedido> Pedido { get; set; } = default!;
    }
}
