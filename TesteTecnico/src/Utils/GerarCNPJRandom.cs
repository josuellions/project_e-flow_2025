namespace TesteTecnico.src.Utils
{
    public static class GerarCNPJRandom
    {
        /// <summary>
        /// Gera CNPJs válidos de forma aleatória, respeitando a estrutura oficial para fins de teste e validação.
        /// </summary>

        private static Random _random = new Random();

        public static string Execute()
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            string semente = "";
            for (int i = 0; i < 8; i++) semente += _random.Next(0, 10);
            semente += "0001"; // Filial padrão

            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            semente += digito1;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return semente + digito2;
        }
    }
}
