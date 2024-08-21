namespace Frenet.ShipManagement.Models
{
    /// <summary>
    /// Contém as classes relacionadas à resposta da API de frete Frenet.
    /// </summary>
    public class ShippingResponseFrenet
    {
        /// <summary>
        /// Representa a resposta da API de frete Frenet, envolvendo a lista de serviços de frete disponíveis.
        /// </summary>
        public class ShippingResponseWrapper
        {
            /// <summary>
            /// Array de respostas de serviços de frete fornecidos pela API.
            /// </summary>
            public ShippingServiceResponse[] ShippingSevicesArray { get; set; }
        }

        /// <summary>
        /// Representa a resposta de um serviço de frete específico fornecido pela API.
        /// </summary>
        public class ShippingServiceResponse
        {
            /// <summary>
            /// Código do serviço de frete.
            /// </summary>
            public string ServiceCode { get; set; }

            /// <summary>
            /// Descrição do serviço de frete.
            /// </summary>
            public string ServiceDescription { get; set; }

            /// <summary>
            /// Nome da transportadora.
            /// </summary>
            public string Carrier { get; set; }

            /// <summary>
            /// Código da transportadora.
            /// </summary>
            public string CarrierCode { get; set; }

            /// <summary>
            /// Preço do frete como uma string.
            /// </summary>
            public string ShippingPrice { get; set; }

            /// <summary>
            /// Tempo de entrega como uma string.
            /// </summary>
            public string DeliveryTime { get; set; }

            /// <summary>
            /// Indica se houve um erro na resposta.
            /// </summary>
            public bool Error { get; set; }

            /// <summary>
            /// Tempo de entrega original como uma string.
            /// </summary>
            public string OriginalDeliveryTime { get; set; }

            /// <summary>
            /// Preço do frete original como uma string.
            /// </summary>
            public string OriginalShippingPrice { get; set; }

            /// <summary>
            /// Tempo de resposta da API como uma string.
            /// </summary>
            public string ResponseTime { get; set; }

            /// <summary>
            /// Indica se a compra de etiquetas é permitida.
            /// </summary>
            public bool AllowBuyLabel { get; set; }

            /// <summary>
            /// Mensagem de resposta da API.
            /// </summary>
            public string Msg { get; set; }
        }
    }
}
