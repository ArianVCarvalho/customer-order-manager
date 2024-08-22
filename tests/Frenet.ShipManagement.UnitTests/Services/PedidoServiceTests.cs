using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Frenet.ShipManagement.DTOs;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Frenet.ShipManagement.UnitTests.Services
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IShippingService> _shippingServiceMock;
        private readonly PedidoService _pedidoService;
        private readonly ILogger<PedidoService> _logger;

        public PedidoServiceTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _shippingServiceMock = new Mock<IShippingService>();

            // Criação do mock do logger
            _logger = new LoggerFactory().CreateLogger<PedidoService>();

            _pedidoService = new PedidoService(_pedidoRepositoryMock.Object, _shippingServiceMock.Object, _logger);
        }

        [Fact]
        public async Task GetPedidos_ShouldReturnListOfPedidos()
        {
            var expectedPedidos = new List<PedidoResponse>
            {
                new PedidoResponse { ClienteId = 1, Pedido = new PedidoDto { Id = 1, Destino = "11111111", Origem = "22222222", Status = Status.Processamento } },
                new PedidoResponse { ClienteId = 2, Pedido = new PedidoDto { Id = 2, Destino = "33333333", Origem = "44444444", Status = Status.Processamento } }
            };

            _pedidoRepositoryMock.Setup(repo => repo.GetPedidos(default))
                .ReturnsAsync(expectedPedidos);

            var result = await _pedidoService.GetPedidos(default);

            result.Should().BeEquivalentTo(expectedPedidos);
            _pedidoRepositoryMock.Verify(repo => repo.GetPedidos(default), Times.Once);
        }

        [Fact]
        public async Task CreatePedido_ShouldCreatePedidoAndCalculateFrete()
        {
            var pedidoRequest = new PedidoRequest
            {
                ClienteId = 1,
                Destino = "11111111",
                Origem = "22222222"
            };

            var freteResponse = new ShippingResponse { ShippingPrice = 100.0m };

            var novoPedido = new Pedido
            {
                ClienteId = pedidoRequest.ClienteId,
                Destino = pedidoRequest.Destino,
                Origem = pedidoRequest.Origem,
                Status = Status.Processamento,
                ValorFrete = freteResponse.ShippingPrice
            };

            _shippingServiceMock.Setup(s => s.CalcularFrete(It.Is<SimulacaoDto>(simulacao =>
                simulacao.Destino == pedidoRequest.Destino &&
                simulacao.Origem == pedidoRequest.Origem
            )))
            .ReturnsAsync(Result<ShippingResponse>.Success(freteResponse));

            _pedidoRepositoryMock.Setup(repo => repo.CreatePedido(It.Is<Pedido>(pedido =>
                pedido.ClienteId == pedidoRequest.ClienteId &&
                pedido.Destino == pedidoRequest.Destino &&
                pedido.Origem == pedidoRequest.Origem &&
                pedido.ValorFrete == freteResponse.ShippingPrice
            )))
            .ReturnsAsync(novoPedido);

            var result = await _pedidoService.CreatePedido(pedidoRequest);

            result.Should().BeEquivalentTo(novoPedido);
            _shippingServiceMock.Verify(s => s.CalcularFrete(It.IsAny<SimulacaoDto>()), Times.Once);
            _pedidoRepositoryMock.Verify(repo => repo.CreatePedido(It.IsAny<Pedido>()), Times.Once);
        }

        [Fact]
        public async Task GetPedidoById_ShouldReturnPedido_WhenPedidoExists()
        {
            var pedidoId = 1;
            var expectedPedido = new Pedido
            {
                Id = pedidoId,
                ClienteId = 1,
                Destino = "11111111",
                Origem = "22222222",
                Status = Status.Processamento
            };

            _pedidoRepositoryMock.Setup(repo => repo.GetPedidoById(pedidoId))
                .ReturnsAsync(expectedPedido);

            var result = await _pedidoService.GetPedidoById(pedidoId);

            result.Should().BeEquivalentTo(expectedPedido);
            _pedidoRepositoryMock.Verify(repo => repo.GetPedidoById(pedidoId), Times.Once);
        }

        [Fact]
        public async Task GetPedidosByClienteId_ShouldReturnPedidosForCliente()
        {
            var clienteId = 1;
            var expectedPedidos = new List<PedidoResponse>
            {
                new PedidoResponse
                {
                    ClienteId = clienteId,
                    Pedido = new PedidoDto
                    {
                        Id = 1,
                        Destino = "11111111",
                        Origem = "22222222",
                        Status = Status.Processamento
                    }
                }
            };

            _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByClienteId(clienteId))
                .ReturnsAsync(expectedPedidos);

            var result = await _pedidoService.GetPedidosByClienteId(clienteId);

            result.Should().BeEquivalentTo(expectedPedidos);
            _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByClienteId(clienteId), Times.Once);
        }

        [Fact]
        public async Task UpdatePedido_ShouldUpdatePedidoAndRecalculateFrete()
        {
            var pedidoId = 1;
            var pedidoRequest = new PedidoRequest
            {
                Destino = "33333333",
                Origem = "44444444"
            };

            var freteResponse = new ShippingResponse { ShippingPrice = 200.0m };

            var pedidoAtualizado = new Pedido
            {
                Id = pedidoId,
                ClienteId = 1,
                Destino = pedidoRequest.Destino,
                Origem = pedidoRequest.Origem,
                Status = Status.Processamento,
                ValorFrete = freteResponse.ShippingPrice
            };

            _shippingServiceMock.Setup(s => s.CalcularFrete(It.Is<SimulacaoDto>(simulacao =>
                simulacao.Destino == pedidoRequest.Destino &&
                simulacao.Origem == pedidoRequest.Origem
            )))
            .ReturnsAsync(Result<ShippingResponse>.Success(freteResponse));

            _pedidoRepositoryMock.Setup(repo => repo.UpdatePedido(pedidoId, pedidoRequest, freteResponse.ShippingPrice))
                .ReturnsAsync(pedidoAtualizado);

            var result = await _pedidoService.UpdatePedido(pedidoId, pedidoRequest);

            result.Should().BeEquivalentTo(pedidoAtualizado);
            _shippingServiceMock.Verify(s => s.CalcularFrete(It.IsAny<SimulacaoDto>()), Times.Once);
            _pedidoRepositoryMock.Verify(repo => repo.UpdatePedido(pedidoId, pedidoRequest, freteResponse.ShippingPrice), Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_ShouldUpdatePedidoStatus()
        {
            var pedidoId = 1;
            var novoStatus = Status.Enviado;
            var pedidoAtualizado = new Pedido
            {
                Id = pedidoId,
                ClienteId = 1,
                Destino = "99999999",
                Origem = "88888888",
                Status = novoStatus
            };

            _pedidoRepositoryMock.Setup(repo => repo.UpdateStatus(pedidoId, novoStatus))
                .ReturnsAsync(pedidoAtualizado);

            var result = await _pedidoService.UpdateStatus(pedidoId, novoStatus);

            result.Should().BeEquivalentTo(pedidoAtualizado);
            _pedidoRepositoryMock.Verify(repo => repo.UpdateStatus(pedidoId, novoStatus), Times.Once);
        }
    }
}
