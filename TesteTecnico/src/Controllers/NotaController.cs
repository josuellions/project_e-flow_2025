using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Communication.Responses;
using TesteTecnico.src.UseCases.GerarXMLNota.GetCurrent;
using TesteTecnico.src.UseCases.GerarXMLNota.PostCurrent;

namespace TesteTecnico.src.Controllers
{
    public class NotaController : Controller
    {
        private readonly PostCurrentGerarXMLNota _useCasePost;
        private readonly GetCurrentListaXMLNota _useCaseGet;

        public NotaController(PostCurrentGerarXMLNota useCasePost, GetCurrentListaXMLNota useCaseGet)
        {
            _useCasePost = useCasePost;
            _useCaseGet = useCaseGet;
        }

        /// <summary>
        /// Responsável por orquestrar as requisições e delegar as operações entre a camada de aplicação e os serviços da lógica de negócio.
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="modelo"></param>
        /// <param name="cnpjDestinatario"></param>
        /// <returns></returns>


        public string GerarXmlNota(string tipo, string modelo, string cnpjDestinatario)
        {
            // FIXME: Lógica incompleta para nota de devolução (quando tipoNota = "devolucao")
            var result = _useCasePost.Execute(tipo, modelo, cnpjDestinatario);

            return result.Result;
        }

        public List<ResponseNotaFiscal> ListaXmlNota(string tipo, string modelo)
        {
            // Tarefa extra: listar XMLs dos últimos 7 dias (não implementado)
            var result = _useCaseGet.ObterNotasUltimos7Dias(tipo, modelo);

            return result.Result;
        }
    }
}
