using TesteTecnico.Communication.Responses;
using TesteTecnico.src.Contracts;

namespace TesteTecnico.src.UseCases.GerarXMLNota.GetCurrent
{
    public class GetCurrentListaXMLNota 
    {
        private readonly INotaRepository _repository;

        public GetCurrentListaXMLNota(INotaRepository repository) => _repository = repository;

        /// <summary>
        /// Implementa a regra de negocio retornar os arquivos XML de notas fiscais gerados e salvos
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="modelo"></param>
        /// <returns></returns>

        // Retorna os arquivos dos últimos 7 dias com base na data embutida na chave
        public async Task<List<ResponseNotaFiscal>> ObterNotasUltimos7Dias(string tipo, string modelo)
        {
            var dataFim = DateTime.Today;
            var dataInicio= dataFim.AddDays(-7);

            var result = await Task.Run(() =>  _repository.ListaPorPeriodo(dataInicio, dataFim, modelo));

            return result;
    
        }
    }
}
