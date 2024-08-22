using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Xunit;
using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories;

namespace Frenet.ShipManagement.IntegrationTests.Repositories
{
    public sealed class ClienteRepositoryTests : IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();
        private ApplicationDbContext _context;
        private ClienteRepository _clienteRepository;

        public async Task InitializeAsync()
        {
            // Inicializa o contêiner SQL Server
            await _msSqlContainer.StartAsync();

            // Configura o DbContext com a string de conexão do contêiner
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .Options;

            _context = new ApplicationDbContext(options);
            _clienteRepository = new ClienteRepository(_context);

            // Aplica as migrations
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
            await _context.DisposeAsync();
        }

        [Fact]
        public async Task Should_Create_And_Retrieve_Cliente()
        {
            // Arrange
            var clienteDto = new ClienteDto
            {
                Nome = "Cliente Teste",
                Endereco = "Rua Exemplo, 123",
                Telefone = "123456789",
                Email = "teste@cliente.com"
            };

            // Act
            var createdCliente = await _clienteRepository.CreateCliente(clienteDto);
            var retrievedCliente = await _clienteRepository.GetClienteById(createdCliente.Id);

            // Assert
            Assert.NotNull(retrievedCliente);
            Assert.Equal(clienteDto.Nome, retrievedCliente.Nome);
            Assert.Equal(clienteDto.Endereco, retrievedCliente.Endereco);
            Assert.Equal(clienteDto.Telefone, retrievedCliente.Telefone);
            Assert.Equal(clienteDto.Email, retrievedCliente.Email);
        }

        [Fact]
        public async Task Should_Update_Cliente()
        {
            // Arrange
            var clienteDto = new ClienteDto
            {
                Nome = "Cliente Original",
                Endereco = "Rua Exemplo, 123",
                Telefone = "123456789",
                Email = "original@cliente.com"
            };

            var createdCliente = await _clienteRepository.CreateCliente(clienteDto);

            var updatedDto = new ClienteDto
            {
                Nome = "Cliente Atualizado",
                Endereco = "Rua Atualizada, 456",
                Telefone = "987654321",
                Email = "atualizado@cliente.com"
            };

            // Act
            var updatedCliente = await _clienteRepository.UpdateCliente(updatedDto, createdCliente.Id);
            var retrievedCliente = await _clienteRepository.GetClienteById(updatedCliente.Id);

            // Assert
            Assert.NotNull(retrievedCliente);
            Assert.Equal(updatedDto.Nome, retrievedCliente.Nome);
            Assert.Equal(updatedDto.Endereco, retrievedCliente.Endereco);
            Assert.Equal(updatedDto.Telefone, retrievedCliente.Telefone);
            Assert.Equal(updatedDto.Email, retrievedCliente.Email);
        }

        [Fact]
        public async Task Should_Delete_Cliente()
        {
            // Arrange
            var clienteDto = new ClienteDto
            {
                Nome = "Cliente para Deletar",
                Endereco = "Rua Exemplo, 123",
                Telefone = "123456789",
                Email = "delete@cliente.com"
            };

            var createdCliente = await _clienteRepository.CreateCliente(clienteDto);

            // Act
            await _clienteRepository.DeleteCliente(createdCliente.Id);
            var retrievedCliente = await _clienteRepository.GetClienteById(createdCliente.Id);

            // Assert
            Assert.Null(retrievedCliente);
        }

        [Fact]
        public async Task Should_Get_All_Clientes()
        {
            // Arrange
            var cliente1 = new ClienteDto
            {
                Nome = "Cliente 1",
                Endereco = "Rua Um, 111",
                Telefone = "111111111",
                Email = "cliente1@teste.com"
            };

            var cliente2 = new ClienteDto
            {
                Nome = "Cliente 2",
                Endereco = "Rua Dois, 222",
                Telefone = "222222222",
                Email = "cliente2@teste.com"
            };

            await _clienteRepository.CreateCliente(cliente1);
            await _clienteRepository.CreateCliente(cliente2);

            // Act
            var clientes = await _clienteRepository.GetClientesAsync(default);

            // Assert
            Assert.NotEmpty(clientes);
            Assert.True(clientes.Count >= 2); // Verifica que pelo menos 2 clientes foram retornados
        }
    }
}
