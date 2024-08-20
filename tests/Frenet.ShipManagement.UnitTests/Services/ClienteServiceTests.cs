using FluentAssertions;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Frenet.ShipManagement.UnitTests.Services
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _clienteService = new ClienteService(_clienteRepositoryMock.Object);
        }

        [Fact]
        public async Task GetClientesAsync_ShouldReturnListOfClientes()
        {
            var expectedClientes = new List<Cliente>
        {
            new Cliente { Id = 1, Nome = "Cliente 1" },
            new Cliente { Id = 2, Nome = "Cliente 2" }
        };

            _clienteRepositoryMock.Setup(repo => repo.GetClientesAsync())
                .ReturnsAsync(expectedClientes);

            var result = await _clienteService.GetClientesAsync();

            result.Should().BeEquivalentTo(expectedClientes);
            _clienteRepositoryMock.Verify(repo => repo.GetClientesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetClienteById_ShouldReturnCliente_WhenClienteExists()
        {
            var clienteId = 1;
            var expectedCliente = new Cliente { Id = clienteId, Nome = "Cliente 1" };

            _clienteRepositoryMock.Setup(repo => repo.GetClienteById(clienteId))
                .ReturnsAsync(expectedCliente);

            var result = await _clienteService.GetClienteById(clienteId);

            var okResult = result as Ok<Cliente>;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(expectedCliente);
            _clienteRepositoryMock.Verify(repo => repo.GetClienteById(clienteId), Times.Once);
        }

        [Fact]
        public async Task GetClienteById_ShouldReturnNotFound_WhenClienteDoesNotExist()
        {
            var clienteId = 999;
            _clienteRepositoryMock.Setup(repo => repo.GetClienteById(clienteId))
                .ReturnsAsync((Cliente)null);

            var result = await _clienteService.GetClienteById(clienteId);

            var notFoundResult = result as NotFound<string>;
            notFoundResult.Should().NotBeNull();
            notFoundResult.Value.Should().Be("Cliente não encontrado");
            _clienteRepositoryMock.Verify(repo => repo.GetClienteById(clienteId), Times.Once);
        }

        [Fact]
        public async Task CreateCliente_ShouldReturnCreatedCliente()
        {
            var clienteDto = new ClienteDto { Nome = "Novo Cliente" };
            var expectedCliente = new Cliente { Id = 1, Nome = "Novo Cliente" };

            _clienteRepositoryMock.Setup(repo => repo.CreateCliente(clienteDto))
                .ReturnsAsync(expectedCliente);

            var result = await _clienteService.CreateCliente(clienteDto);


            result.Should().BeEquivalentTo(expectedCliente);
            _clienteRepositoryMock.Verify(repo => repo.CreateCliente(clienteDto), Times.Once);
        }

        [Fact]
        public async Task UpdateCliente_ShouldReturnUpdatedCliente()
        {
            var clienteDto = new ClienteDto { Nome = "Cliente Atualizado" };
            var updatedCliente = new Cliente { Id = 1, Nome = "Cliente Atualizado" };

            _clienteRepositoryMock.Setup(repo => repo.UpdateCliente(clienteDto, 1))
                .ReturnsAsync(updatedCliente);

            var result = await _clienteService.UpdateCliente(clienteDto, 1);

            result.Should().BeEquivalentTo(updatedCliente);
            _clienteRepositoryMock.Verify(repo => repo.UpdateCliente(clienteDto, 1), Times.Once);
        }

        [Fact]
        public async Task DeleteCliente_ShouldDeleteCliente_WhenClienteExists()
        {
            var clienteId = 1;

            _clienteRepositoryMock.Setup(repo => repo.DeleteCliente(clienteId))
                .Returns(Task.CompletedTask);

            await _clienteService.DeleteCliente(clienteId);

            _clienteRepositoryMock.Verify(repo => repo.DeleteCliente(clienteId), Times.Once);
        }
    }

}
