using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Frenet.ShipManagement.Models;

namespace Frenet.ShipManagement.Data
{
    public class FrenetShipManagementContext : DbContext
    {
        public FrenetShipManagementContext (DbContextOptions<FrenetShipManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Frenet.ShipManagement.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<Frenet.ShipManagement.Models.Pedido> Pedido { get; set; } = default!;
    }
}
