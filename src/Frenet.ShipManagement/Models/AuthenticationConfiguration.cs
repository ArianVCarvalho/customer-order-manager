namespace Frenet.ShipManagement.Models
{
    /// <summary>
    /// Configurações de autenticação para o sistema, utilizadas na geração e validação de tokens.
    /// </summary>
    public class AuthenticationConfiguration
    {
        /// <summary>
        /// Público-alvo (Audience) para o qual o token é emitido.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Emissor (Issuer) do token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Chave secreta utilizada para a assinatura e verificação do token.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Tempo de expiração do token em minutos.
        /// </summary>
        public int ExpiresInMinutes { get; set; }
    }
}
