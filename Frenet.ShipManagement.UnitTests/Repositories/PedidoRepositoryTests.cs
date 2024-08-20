using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Frenet.ShipManagement.Repositories;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Views.Dto;
using Frenet.ShipManagement.Views.Request;
using Frenet.ShipManagement.Views.Response;
using Frenet.ShipManagement.Data;

namespace Frenet.ShipManagement.UnitTests.Repositories
{
    public class PedidoRepositoryTests
    {
        private readonly PedidoRepository _pedidoRepository;
        private readonly FrenetShipManagementContext _context;

        public PedidoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FrenetShipManagementContext>()
                .UseInMemoryDatabase("FrenetShipManagementTest")
                .Options;

            _context = new FrenetShipManagementContext(options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, Origem = "12345678", Destino = "87654321", DataCriacao = DateTime.UtcNow, Status = Status.Processamento, ValorFrete = 10.0M, ClienteId = 1 },
                new Pedido { Id = 2, Origem = "23456789", Destino = "98765432", DataCriacao = DateTime.UtcNow.AddDays(-1), Status = Status.Enviado, ValorFrete = 20.0M, ClienteId = 2 }
            };

            _context.Pedido.AddRange(pedidos);
            _context.SaveChanges();

            _pedidoRepository = new PedidoRepository(_context);
        }

        [Fact]
        public async Task GetPedidos_ShouldReturnListOfPedidos()
        {
            var result = await _pedidoRepository.GetPedidos();

            result.Should().HaveCount(2);
            result.First().Pedido.Id.Should().Be(1);
            result.Last().Pedido.Id.Should().Be(2);
        }

        [Fact]
        public async Task CreatePedido_ShouldAddPedidoToContext()
        {
            var newPedido = new Pedido { Id = 3, Origem = "34567890", Destino = "09876543", DataCriacao = DateTime.UtcNow, Status = Status.Processamento, ValorFrete = 30.0M, ClienteId = 3 };

            var result = await _pedidoRepository.CreatePedido(newPedido);

            var pedidoNoContext = await _context.Pedido.FindAsync(3);
            pedidoNoContext.Should().NotBeNull();
            pedidoNoContext.Should().BeEquivalentTo(newPedido);
        }

        [Fact]
        public async Task VerifyDatabaseContent()
        {
            var pedidos = await _context.Pedido.ToListAsync();
            pedidos.Should().HaveCount(2); 
            pedidos.Should().Contain(p => p.Id == 1); 
        }

        [Fact]
        public async Task GetPedidoById_ShouldReturnPedido_WhenExists()
        {
            var result = await _pedidoRepository.GetPedidoById(1);

            result.Should().NotBeNull("because the pedido with ID 1 should exist in the database");
            result.Id.Should().Be(1, "because the ID of the returned pedido should be 1");
        }

        [Fact]
        public async Task GetPedidoById_ShouldReturnNull_WhenNotExists()
        {
            var result = await _pedidoRepository.GetPedidoById(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPedidosByClienteId_ShouldReturnListOfPedidos()
        {
            var clienteId = 1;
            var pedido = new Pedido
            {
                Id = 1, 
                Origem = "12345678",
                Destino = "87654321",
                DataCriacao = DateTime.UtcNow,
                Status = Status.Processamento,
                ValorFrete = 10.0M,
                ClienteId = clienteId
            };

            var existingPedido = await _context.Pedido.FindAsync(pedido.Id);
            if (existingPedido == null)
            {
                _context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();
            }

            // Act
            var result = await _pedidoRepository.GetPedidosByClienteId(clienteId);

            // Assert
            result.Should().HaveCount(1);
            result.First().ClienteId.Should().Be(clienteId);
        }

        [Fact]
        public async Task UpdatePedido_ShouldUpdatePedido_WhenExists()
        {
            var pedidoDto = new PedidoRequest
            {
                Origem = "UpdatedOrigin",
                Destino = "UpdatedDestination"
            };

            var updatedPedido = await _pedidoRepository.UpdatePedido(1, pedidoDto, 15.0M);

            updatedPedido.Should().NotBeNull();
            updatedPedido.Origem.Should().Be("UpdatedOrigin");
            updatedPedido.Destino.Should().Be("UpdatedDestination");
            updatedPedido.ValorFrete.Should().Be(15.0M);
        }

        [Fact]
        public async Task UpdatePedido_ShouldReturnNull_WhenNotExists()
        {
            var result = await _pedidoRepository.UpdatePedido(999, new PedidoRequest(), 15.0M);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateStatus_ShouldUpdateStatus_WhenExists()
        {
            var updatedPedido = await _pedidoRepository.UpdateStatus(1, Status.Enviado);

            updatedPedido.Should().NotBeNull();
            updatedPedido.Status.Should().Be(Status.Enviado);
        }

        [Fact]
        public async Task UpdateStatus_ShouldReturnNull_WhenNotExists()
        {
            var result = await _pedidoRepository.UpdateStatus(999, Status.Cancelado);

            result.Should().BeNull();
        }
    }
}
