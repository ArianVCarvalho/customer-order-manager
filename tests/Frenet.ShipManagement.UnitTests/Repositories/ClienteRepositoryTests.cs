using FluentAssertions;
using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Frenet.ShipManagement.UnitTests.Repositories
{
    public class ClienteRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ClienteRepository _clienteRepository;

        public ClienteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("FrenetShipManagementTest")
                .EnableSensitiveDataLogging()
                .Options;

            _context = new ApplicationDbContext(options);
            _clienteRepository = new ClienteRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetClientesAsync_ShouldReturnListOfClientes()
        {
            var cliente = new Cliente { Nome = "Cliente Teste", Telefone = "1234567890", Endereco = "Rua Teste", Email = "teste@teste.com" };
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            var result = await _clienteRepository.GetClientesAsync();

            result.Should().HaveCount(1);
            result[0].Nome.Should().Be("Cliente Teste");
        }

        [Fact]
        public async Task GetClienteById_ShouldReturnCliente_WhenExists()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "Cliente Teste",
                Telefone = "123456789",
                Endereco = "Rua Teste",
                Email = "cliente@teste.com"
            };

            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            var clienteSalvo = await _context.Cliente.FindAsync(cliente.Id);
            clienteSalvo.Should().NotBeNull();  

            var result = await _clienteRepository.GetClienteById(cliente.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(cliente.Id);
            result.Nome.Should().Be(cliente.Nome);
            result.Telefone.Should().Be(cliente.Telefone);
            result.Endereco.Should().Be(cliente.Endereco);
            result.Email.Should().Be(cliente.Email);
        }



        [Fact]
        public async Task GetClienteById_ShouldReturnNull_WhenNotExists()
        {
            var result = await _clienteRepository.GetClienteById(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateCliente_ShouldCreateAndReturnCliente()
        {
            var clienteDto = new ClienteDto { Nome = "Novo Cliente", Telefone = "0987654321", Endereco = "Nova Rua", Email = "novo@teste.com" };

            var result = await _clienteRepository.CreateCliente(clienteDto);

            result.Should().NotBeNull();
            result.Nome.Should().Be("Novo Cliente");
        }

        [Fact]
        public async Task UpdateCliente_ShouldUpdateAndReturnCliente_WhenExists()
        {
            var cliente = new Cliente { Nome = "Cliente Original", Telefone = "1234567890", Endereco = "Rua Original", Email = "original@teste.com" };
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            var clienteDto = new ClienteDto { Nome = "Cliente Atualizado", Telefone = "0987654321", Endereco = "Rua Atualizada", Email = "atualizado@teste.com" };

            var result = await _clienteRepository.UpdateCliente(clienteDto, cliente.Id);

            result.Should().NotBeNull();
            result.Nome.Should().Be("Cliente Atualizado");
        }

        [Fact]
        public async Task UpdateCliente_ShouldThrowException_WhenNotExists()
        {
            var clienteDto = new ClienteDto { Nome = "Cliente Atualizado", Telefone = "0987654321", Endereco = "Rua Atualizada", Email = "atualizado@teste.com" };

            Func<Task> act = async () => await _clienteRepository.UpdateCliente(clienteDto, 999);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteCliente_ShouldDeleteCliente_WhenExists()
        {
            var cliente = new Cliente { Nome = "Cliente para Excluir", Telefone = "1234567890", Endereco = "Rua Excluir", Email = "excluir@teste.com" };
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            await _clienteRepository.DeleteCliente(cliente.Id);

            var result = await _clienteRepository.GetClienteById(cliente.Id);
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCliente_ShouldThrowException_WhenNotExists()
        {
            Func<Task> act = async () => await _clienteRepository.DeleteCliente(999);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
