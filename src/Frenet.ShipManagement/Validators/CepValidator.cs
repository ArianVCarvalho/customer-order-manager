namespace Frenet.ShipManagement.Validators
{
    public static class CepValidator
    {
        /// <summary>
        /// Valida se o CEP contém apenas números e tem exatamente 8 caracteres.
        /// </summary>
        /// <param name="cep">CEP a ser validado.</param>
        /// <returns>Verdadeiro se o CEP for válido, falso caso contrário.</returns>
        public static bool IsValid(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
                return false;

            // Verifica se todos os caracteres são números
            return long.TryParse(cep, out _);
        }
    }
}
