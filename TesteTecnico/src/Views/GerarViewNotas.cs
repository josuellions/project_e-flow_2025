using TesteTecnico.Communication.Responses;

namespace TesteTecnico.src.Views
{
    public class GerarViewNotas
    {
        /// <summary>
        /// Monta a visualização estruturada de notas fiscais processadas, facilitando a análise e exibição dos dados restorno.
        /// </summary>
        /// <param name="result"></param>
        /// 

        public static void Lista(List<ResponseNotaFiscal> result)
        {
            bool header = true;
            int lenData = 0;
            int lenCnpj = 0;
            int lenChave = 0;
            int lenArquivo = 0;

            foreach (var item in result)
            {
                string cnpjFormat = Convert.ToUInt64(item.CNPJ).ToString(@"00\.000\.000\/0000\-00");

                if (header)
                {
                    // Cabeçalho lista notas
                    string dataHeader = "Data";
                    string cnpjHeader = "CNPJ";
                    string chaveHeader = "Chave";
                    string nomeHeader = "Arquivo";

                    lenData = item.Date.ToString("dd/MM/yyyy").Length;
                    lenCnpj = cnpjFormat.Length;
                    lenChave = item.Chave.Length;
                    lenArquivo = item.FileName.Length;

                    #region Header
                    Console.WriteLine("");
                    Console.WriteLine($"|-{new string('-', lenData)}---{new string('-', lenCnpj)}---{new string('-', lenChave)}---{new string('-', lenArquivo)}-|");
                    Console.WriteLine($"| {dataHeader.PadRight(lenData)} | {cnpjHeader.PadRight(lenCnpj)} | {chaveHeader.PadRight(lenChave)} | {nomeHeader.PadRight(lenArquivo)} |");
                    Console.WriteLine($"|-{new string('-', lenData)}-+-{new string('-', lenCnpj)}-+-{new string('-', lenChave)}-+-{new string('-', lenArquivo)}-|");
                    #endregion
                    header = false;
                }

                #region Body

                Console.WriteLine($"| {item.Date:dd/MM/yyyy} | {cnpjFormat} | {item.Chave} | {item.FileName} |");

                #endregion
            }
            #region Fotter
            string totalFooter = "Total Notas: ";
            int lenTotalFooter = totalFooter.Length;
            int lenResult = lenData + lenChave + lenArquivo + lenTotalFooter;
            Console.WriteLine($"|-{new string('-', lenData)}-+-{new string('-', lenCnpj)}-+-{new string('-', lenChave)}-+-{new string('-', lenArquivo)}-|");

            Console.WriteLine($"| {totalFooter.PadRight(lenTotalFooter)} {result.Count.ToString().PadRight(lenResult)} |");

            Console.WriteLine($"|-{new string('-', lenData)}---{new string('-', lenCnpj)}---{new string('-', lenChave)}---{new string('-', lenArquivo)}-|");
            #endregion

        }
    }
}
