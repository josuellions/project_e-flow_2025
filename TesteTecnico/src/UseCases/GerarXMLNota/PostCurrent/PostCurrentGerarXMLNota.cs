using System.Xml.Linq;
using TesteTecnico.src.Contracts;

namespace TesteTecnico.src.UseCases.GerarXMLNota.PostCurrent
{
    public class PostCurrentGerarXMLNota
    {
        private readonly INotaRepository _repository;

        public PostCurrentGerarXMLNota(INotaRepository repository) => _repository = repository;


        /// <summary>
        /// Implementa a regra de negocio validando tipo nota enviando dados para repository
        /// </summary>
        /// <param name="tipoNota"></param>
        /// <param name="modelo"></param>
        /// <param name="cnpjDestinatario"></param>
        /// <returns></returns>
        /// 

        public async Task<string> Execute(string tipoNota, string modelo, string cnpjDestinatario)
        {
            // FIXME: Lógica incompleta para nota de devolução (quando tipoNota = "devolucao")
            var nfe = await Task.Run(() => new XElement("NFe",
                new XElement("infNFe",
                new XElement("ide",
                        new XElement("mod", modelo),
                        new XElement("tpNF", tipoNota == "saida" ? "1" : "0"), // 0 = entrada | 1 = saida
                        new XElement("finNFe", tipoNota == "saida" ? "4" : "1") // 1 = emitida | 4 = devolução
                    ),
                    new XElement("dest",
                        new XElement("CNPJ", cnpjDestinatario)
                    )
                )
            ));
            string tipoEmissao = tipoNota == "saida" ? "1" : "0";
            var result = await Task.Run(() => _repository.Create(cnpjDestinatario, modelo, tipoEmissao,  nfe));

            return result;
        }
    }
}
