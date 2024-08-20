using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.Views.Dto;
using Frenet.ShipManagement.Views.Request;
using Frenet.ShipManagement.Views.Response;

namespace Frenet.ShipManagement.Services
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios relacionada aos pedidos.
    /// </summary>
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IShippingService _shippingService;

        /// <summary>
        /// Inicializa uma nova instância do serviço de pedidos.
        /// </summary>
        /// <param name="repository">Repositório de pedidos utilizado pelo serviço.</param>
        /// <param name="shippingService">Serviço de cálculo de frete utilizado pelo serviço.</param>
        public PedidoService(IPedidoRepository repository, IShippingService shippingService)
        {
            this.pedidoRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _shippingService = shippingService ?? throw new ArgumentNullException(nameof(shippingService));
        }

        /// <summary>
        /// Obtém uma lista dos 10 pedidos mais recentes.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="PedidoResponse"/> representando os pedidos mais recentes.</returns>
        public async Task<List<PedidoResponse>> GetPedidos()
        {
            return await pedidoRepository.GetPedidos();
        }

        /// <summary>
        /// Cria um novo pedido com base nas informações fornecidas.
        /// </summary>
        /// <param name="pedido">Objeto <see cref="PedidoRequest"/> contendo as informações do novo pedido.</param>
        /// <returns>O objeto <see cref="Pedido"/> criado.</returns>
        public async Task<Pedido> CreatePedido(PedidoRequest pedido)
        {
            var simulacao = new SimulacaoDto
            {
                Destino = pedido.Destino,
                Origem = pedido.Origem,
            };

            var frete = await _shippingService.CalcularFrete(simulacao);

            var criarPedido = new Pedido
            {
                ClienteId = pedido.ClienteId,
                Destino = pedido.Destino,
                Status = Status.Processamento,
                Origem = pedido.Origem,
                ValorFrete = frete.Value.ShippingPrice // Corrigido para acessar o valor corretamente
            };

            return await pedidoRepository.CreatePedido(criarPedido);
        }

        /// <summary>
        /// Obtém um pedido pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do pedido.</param>
        /// <returns>O objeto <see cref="Pedido"/> correspondente ao identificador fornecido.</returns>
        public async Task<Pedido> GetPedidoById(int id)
        {
            return await pedidoRepository.GetPedidoById(id);
        }

        /// <summary>
        /// Obtém uma lista de pedidos associados a um cliente específico.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente.</param>
        /// <returns>Uma lista de objetos <see cref="Pedido"/> associados ao cliente especificado.</returns>
        public async Task<List<PedidoResponse>> GetPedidosByClienteId(int clienteId)
        {
            return await pedidoRepository.GetPedidosByClienteId(clienteId);
        }

        /// <summary>
        /// Atualiza um pedido existente com as informações fornecidas.
        /// </summary>
        /// <param name="id">Identificador do pedido a ser atualizado.</param>
        /// <param name="pedido">Objeto <see cref="PedidoDto"/> contendo as novas informações do pedido.</param>
        /// <returns>O objeto <see cref="Pedido"/> atualizado.</returns>
        public async Task<Pedido> UpdatePedido(int id, PedidoRequest pedido)
        {
            var simulacao = new SimulacaoDto
            {
                Destino = pedido.Destino,
                Origem = pedido.Origem,
            };

            var frete = await _shippingService.CalcularFrete(simulacao);

            return await pedidoRepository.UpdatePedido(id, pedido, frete.Value.ShippingPrice);
        }
        public async Task<Pedido> UpdateStatus(int id, Status status)
        {
            return await pedidoRepository.UpdateStatus(id, status);
        }
    }
}
