using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TesteTecnico.Communication.Responses;
using TesteTecnico.src.Contracts;
using TesteTecnico.src.Utils;

namespace TesteTecnico.src.Repositories.DataAccess
{
    public class NotaRepository : INotaRepository
    {
        // Caminho global do diretório de logs, relativo à raiz do projeto
        private static readonly string DIR_BASE = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..")));

        // Caminho relativo ao diretório do projeto
        private static string ObterDiretorioNotas()
        {
            return Path.Combine(
               DIR_BASE,
                "Public", "Files", "Notas"
            );
        }

        /// <summary>
        /// Responsável por persistir dados em arquivos XML no repositório da aplicação.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="cnpj"></param>
        /// <param name="chave"></param>
        /// <param name="fileName"></param>

        private static void SalveLoggerNFe(string date, string cnpj, string chave, string fileName)
        {
            // Montar linha
            string linha = $"{date} | {cnpj} | {chave} | {fileName} ";

            // Caminho arquivo save log
            string file = $"{DateTime.Now:yyyyMMdd}_xmllogs.txt";
            string directory = Path.Combine(DIR_BASE, "App_Data", "Files", "Logs");

            // Cria o diretório (caso não exista)
            Directory.CreateDirectory(directory);
            string caminho = Path.Combine(directory, file);

            bool arquivoExist = !File.Exists(caminho);

            // Salvar em modo append
            using (StreamWriter writer = new StreamWriter(caminho, append: true))
            {
                int wData = Math.Max("Data".Length, date.Length);
                int wCNPJ = Math.Max("CNPJ".Length, cnpj.Length);
                int wChave = Math.Max("Chave".Length, chave.Length);
                int wArquivo = Math.Max("Arquivo".Length, fileName.Length);

                if (arquivoExist)
                {
                    // Escreve cabeçalho apenas na criação
                    // Cabeçalho dinâmico
                    string header = $"{"Data".PadRight(wData)} | {"CNPJ".PadRight(wCNPJ)} | {"Chave".PadRight(wChave)} | {"Arquivo".PadRight(wArquivo)}";
                    writer.WriteLine(header);
                }
                // Calcular larguras com base nos dados (ou definir mínimas)
                // Formatar linha de dados
                string linhaDados = $"{date.PadRight(wData)} | {cnpj.PadRight(wCNPJ)} | {chave.PadRight(wChave)} | {fileName.PadRight(wArquivo)}";

                // Escreve a linha de dados
                writer.WriteLine(linhaDados);
            }
        }

        private static List<ResponseNotaFiscal> LerXmlPorPeriodoLogs(DateTime dataInicio, DateTime dataFim)
        {
            List<ResponseNotaFiscal> resultados = new();
            string directory = Path.Combine(DIR_BASE, "App_Data", "Files", "Logs");
            if (!Directory.Exists(directory))
                return resultados;


            if (!Directory.Exists(directory))
                return resultados;

            var arquivos = Directory.GetFiles(directory, "*_xmllogs.txt");

            foreach (var arquivo in arquivos)
            {
                string nome = Path.GetFileName(arquivo);
                var match = Regex.Match(nome, @"^(\d{8})_xmllogs\.txt$");

                if (match.Success && DateTime.TryParseExact(match.Groups[1].Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataArquivo))
                {
                    if (dataArquivo >= dataInicio && dataArquivo <= dataFim)
                    {
                        var linhas = File.ReadAllLines(arquivo);

                        for (int i = 2; i < linhas.Length; i++) // Pula cabeçalho
                        {
                            var linha = linhas[i].Trim();
                            if (string.IsNullOrWhiteSpace(linha)) continue;

                            // Regex para extrair os 4 campos da linha
                            var campos = linha.Split('|')
                              .Select(p => p.Trim())
                              .Where(p => !string.IsNullOrWhiteSpace(p))
                              .ToArray();

                            if (campos.Length >= 4 &&
                            DateTime.TryParseExact(campos[0].Trim(), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataNota))
                            {
                                string cnpj = campos[1].Replace(".", "").Replace("/", "").Replace("-", ""); // remove formatação CNPJ
                                string chave = campos[2];
                                string fileName = campos[3];

                                if (dataNota >= dataInicio && dataNota <= dataFim)
                                {
                                    resultados.Add(new ResponseNotaFiscal
                                    {
                                        Date = dataNota,
                                        CNPJ = cnpj,
                                        Chave = chave,
                                        FileName = fileName
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return resultados;
        }

        private static List<ResponseNotaFiscal> LerXmlPorPeriodoChave(DateTime dataInicio, DateTime dataFim)
        {

            var publicPath = ObterDiretorioNotas();

            if (!Directory.Exists(publicPath))
                return new List<ResponseNotaFiscal>();

            var arquivos = Directory.GetFiles(publicPath, "*.xml");

            var resultado = new List<ResponseNotaFiscal>();
            var arquivosValidos = new List<(string path, DateTime data)>();

            foreach (var arquivo in arquivos)
            {
                string fileName = Path.GetFileName(arquivo);

                // Extrai a primeira sequência com exatamente 44 dígitos
                var matchChave = Regex.Match(fileName, @"\d{44}");

                if (!matchChave.Success) continue;

                string chave = matchChave.Value;

                // Extrai AAMM da chave (posições 3 a 6 → índices 2 a 5)
                string aaMm = chave.Substring(2, 4);

                // Extri CNPJ da chave
                string cnpj = chave.Substring(6, 14);

                try
                {
                    string aaMmDd = aaMm + dataInicio.Day.ToString("D2"); // ex: "250710"
                    DateTime dataNota = DateTime.ParseExact(aaMmDd, "yyMMdd", CultureInfo.InvariantCulture);

                    if (dataNota >= dataInicio && dataNota <= dataFim)
                    {
                        resultado.Add(new ResponseNotaFiscal
                        {
                            Date = dataNota,
                            CNPJ = cnpj,
                            Chave = chave,
                            FileName = fileName
                        });
                    }
                }
                catch
                {
                    continue; // ignora erros de data
                }
            }

            // Ordena por data decrescente
            return resultado
                .OrderByDescending(f => f.Date)
                .ToList();
        }

        public string Create(string cnpj, string modelo, string tipoEmissao, XElement xml)
        {
            var filePath = string.Empty;
            try
            {
                var publicPath = ObterDiretorioNotas();
                Directory.CreateDirectory(publicPath);

                int anoInico = 2023;
                int anoFim = 2025;
                var dateRandom = GerarDateRandom.GerarDataAleatoria(anoInico, anoFim);

                var chaveRandom = GerarChaveNFeRandom.Execute(dateRandom, cnpj, modelo, tipoEmissao);

                filePath = Path.Combine(publicPath, $"{chaveRandom}.xml");
                File.WriteAllText(filePath, xml.ToString());

                string fileName = Path.GetFileName(filePath);

                File.WriteAllText(filePath, xml.ToString());

                SalveLoggerNFe(dateRandom.ToString("ddMMyyyy"), cnpj, chaveRandom, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao gerar XML: " + ex.Message);
            }

            return Path.GetFileName(filePath);
        }

        public List<ResponseNotaFiscal> ListaPorPeriodo(DateTime dataInicio, DateTime dataFim, string modelo)
        {
            var resultado = new List<ResponseNotaFiscal>();
            //var arquivosValidos = new List<(string path, DateTime data)>();

            var arquivos = LerXmlPorPeriodoLogs(dataInicio, dataFim);
            //var arquivos = LerXmlPorPeriodoChave(dataInicio, dataFim);

            // Ordena por data decrescente
            return arquivos
                .OrderByDescending(f => f.Date)
                .ToList();
        }
    }
}
