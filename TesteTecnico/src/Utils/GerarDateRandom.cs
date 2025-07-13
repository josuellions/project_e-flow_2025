namespace TesteTecnico.src.Utils
{
    public class GerarDateRandom
    {
        /// <summary>
        /// Cria datas aleatórias dentro de um intervalo definido, útil para simulações e testes com dados temporais.
        /// </summary>
        /// <param name="anoInicio"></param>
        /// <param name="anoFim"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>

        public static DateTime GerarDataAleatoria(int anoInicio, int anoFim)
        {
            if (anoFim < anoInicio)
                throw new ArgumentException("Ano final não pode ser menor que o ano inicial.");

            var random = new Random();
            DateTime hoje = DateTime.Today;

            DateTime data;

            do
            {
                int ano = random.Next(anoInicio, anoFim + 1);
                int mes = random.Next(1, 13); // 1 a 12
                int diaMax = DateTime.DaysInMonth(ano, mes);
                int dia = random.Next(1, diaMax + 1);

                data = new DateTime(ano, mes, dia);

            } while (data > hoje); 

            return data;
        }
    }
}
