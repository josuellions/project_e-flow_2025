using System.Xml.Linq;
using TesteTecnico.Communication.Responses;

namespace TesteTecnico.src.Contracts
{
    /// <summary>
    /// Define o contrato para operações de persistência e consulta de notas fiscais, como criação de arquivos XML e listagem por período.
    /// </summary>

    public interface INotaRepository
    {
        string Create(string cnpj, string modelo, string tipoEmissao, XElement xml);
        public List<ResponseNotaFiscal> ListaPorPeriodo(DateTime dataInicio, DateTime dataFim, string modelo);
    }
}
