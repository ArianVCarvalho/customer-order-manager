using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Xunit;
using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories;
using Frenet.ShipManagement.ViewModels;
using Frenet.ShipManagement.DTOs;

namespace Frenet.ShipManagement.IntegrationTests.Repositories
{
    public sealed class PedidoRepositoryTests : IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer
            = new MsSqlBuilder().Build();

        private ApplicationDbContext _context;
        private PedidoRepository _pedidoRepository;
        private ClienteRepository _clienteRepository;

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .Options;

            _context = new ApplicationDbContext(options);
            _pedidoRepository = new PedidoRepository(_context);
            _clienteRepository = new ClienteRepository(_context);

            await _context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
            await _context.DisposeAsync();
        }

        [Fact]
        public async Task Should_Insert_And_Retrieve_Pedido()
        {
            var cliente = new ClienteDto
            {
                Nome = "Cliente Teste",
                Endereco = "Rua Exemplo, 123",
                Telefone = "123456789",
                Email = "teste@cliente.com"
            };

            var newCliente = await _clienteRepository.CreateCliente(cliente);


            var pedido = new Pedido
            {
                ClienteId = newCliente.Id,
                Origem = "12345678", // CEP de origem
                Destino = "87654321", // CEP de destino
                DataCriacao = DateTime.UtcNow,
                Status = Status.Processamento,
                ValorFrete = 100.0m
            };

            var createdPedido = await _pedidoRepository.CreatePedido(pedido);
            var retrievedPedido = await _pedidoRepository.GetPedidoById(createdPedido.Id);

            Assert.NotNull(retrievedPedido);
            Assert.Equal(pedido.ClienteId, retrievedPedido.ClienteId);
            Assert.Equal(pedido.Origem, retrievedPedido.Origem);
            Assert.Equal(pedido.Destino, retrievedPedido.Destino);
            Assert.Equal(pedido.Status, retrievedPedido.Status);
            Assert.Equal(pedido.ValorFrete, retrievedPedido.ValorFrete);
        }

        [Fact]
        public async Task Should_Update_Pedido()
        {
            var cliente = new ClienteDto
            {
                Nome = "Cliente Teste",
                Endereco = "Rua Exemplo, 123",
                Telefone = "123456789",
                Email = "teste@cliente.com"
            };

            var newCliente = await _clienteRepository.CreateCliente(cliente);
            await _context.SaveChangesAsync();

            var pedido = new Pedido
            {
                ClienteId = newCliente.Id,
                Origem = "12345678",
                Destino = "87654321",
                DataCriacao = DateTime.UtcNow,
                Status = Status.Processamento,
                ValorFrete = 100.0m
            };

            var createdPedido = await _pedidoRepository.CreatePedido(pedido);

            var updatedPedidoDto = new PedidoRequest
            {
                Origem = "11111111", // Novo CEP de origem
                Destino = "22222222"  // Novo CEP de destino
            };
            decimal novoFrete = 150.0m;

            var updatedPedido = await _pedidoRepository.UpdatePedido(createdPedido.Id, updatedPedidoDto, novoFrete);
            var retrievedPedido = await _pedidoRepository.GetPedidoById(updatedPedido.Id);

            Assert.NotNull(retrievedPedido);
            Assert.Equal(updatedPedidoDto.Origem, retrievedPedido.Origem);
            Assert.Equal(updatedPedidoDto.Destino, retrievedPedido.Destino);
            Assert.Equal(novoFrete, retrievedPedido.ValorFrete);
        }

        [Fact]
        public async Task Should_Delete_Pedido()
        {
            var cliente = new ClienteDto
            {
                Nome = "Cliente Teste",
                Endereco = "Rua Exemplo, 123",
                Telefone = "123456789",
                Email = "teste@cliente.com"
            };

            var newCliente = await _clienteRepository.CreateCliente(cliente);

            await _context.SaveChangesAsync();

            var pedido = new Pedido
            {
                ClienteId = newCliente.Id,
                Origem = "12345678",
                Destino = "87654321",
                DataCriacao = DateTime.UtcNow,
                Status = Status.Processamento,
                ValorFrete = 100.0m
            };

            var createdPedido = await _pedidoRepository.CreatePedido(pedido);

            await _pedidoRepository.Remove(createdPedido);

            var retrievedPedido = await _pedidoRepository.GetPedidoById(createdPedido.Id);
            Assert.Null(retrievedPedido);
        }
    }
}
