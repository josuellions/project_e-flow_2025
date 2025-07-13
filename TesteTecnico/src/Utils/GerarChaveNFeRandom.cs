namespace TesteTecnico.src.Utils
{
    public static class GerarChaveNFeRandom
    {
        /// <summary>
        /// Gera chaves eletrônicas de NF-e válidas e estruturadas conforme o padrão da SEFAZ, de forma aleatória para fins de teste ou simulação.
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>

        // Calcula dígito verificador da chave pelo módulo 11
        private static int CalcularDigitoVerificador(string chave)
        {
            int soma = 0;
            int peso = 2;

            for (int i = chave.Length - 1; i >= 0; i--)
            {
                soma += int.Parse(chave[i].ToString()) * peso;
                peso = peso < 9 ? peso + 1 : 2;
            }

            int resto = soma % 11;

            return (resto == 0 || resto == 1) ? 0 : 11 - resto;
        }

        // Gera a chave da NFe
        private static string GerarChaveNFe(DateTime dataEmissao, string cnpj, string modelo, string tipoEmissao, string uf, string serie)
        {
            Random _random = new Random();

            string anoMes = dataEmissao.ToString("yyMM");
            string numeroNota = _random.Next(1, 1000000000).ToString("D9");
            string codigoNumerico = _random.Next(0, 100000000).ToString("D8");

            string chaveSemDV =
                uf.PadLeft(2, '0') +
                anoMes +
                cnpj.PadLeft(14, '0') +
                modelo.PadLeft(2, '0') +
                serie.PadLeft(3, '0') +
                numeroNota +
                tipoEmissao +
                codigoNumerico;

            if (chaveSemDV.Length != 43)
                throw new InvalidOperationException("Chave base inválida. Deve conter exatamente 43 dígitos.");

            int dv = CalcularDigitoVerificador(chaveSemDV);

            string chave = chaveSemDV + dv.ToString(); // 44 dígitos finais

            return chave;
        }

        public static string Execute(DateTime date, string cnpj = "", string modelo = "", string tipoEmissao="1",  string uf = "35", string serie = "001")
        {

            return GerarChaveNFe(date, cnpj, modelo, tipoEmissao, uf , serie);
        }
    }
}
