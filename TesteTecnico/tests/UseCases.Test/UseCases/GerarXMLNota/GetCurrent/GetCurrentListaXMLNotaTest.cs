
using Bogus;
using FluentAssertions;
using Moq;
using TesteTecnico.Communication.Responses;
using TesteTecnico.src.Contracts;
using TesteTecnico.src.UseCases.GerarXMLNota.GetCurrent;
using TesteTecnico.src.Utils;

namespace UseCases.Test.UseCases.GerarXMLNota.GetCurrent
{


    public class GetCurrentListaXMLNotaTest
    {
        private static List<ResponseNotaFiscal> GerarDadosFaker(string cnpj, string tipoEmissao, string modelo)
        {
            int anoFim = 2025;
            int anoInicio = 2022;

            DateTime dateFim = DateTime.Now;
            DateTime dateInicio = dateFim.AddDays(-7);
            DateTime date = GerarDateRandom.GerarDataAleatoria(anoInicio, anoFim);

            string tipo = tipoEmissao == "saida" ? "1" : "0";

            var ListaNotasMock = new List<ResponseNotaFiscal> { new ResponseNotaFiscal {
                    Date = date,
            CNPJ = cnpj,
                    Chave = GerarChaveNFeRandom.Execute(date,cnpj, modelo, tipo),
                    FileName = $"{GerarChaveNFeRandom.Execute(date,cnpj, modelo, tipo)}.xml"
                },
                new ResponseNotaFiscal {
                    Date = date,
                    CNPJ = cnpj,
                    Chave = GerarChaveNFeRandom.Execute(date,cnpj, modelo, tipo),
                    FileName = $"{GerarChaveNFeRandom.Execute(date,cnpj, modelo, tipo)}.xml"
                }
            };

            return ListaNotasMock;
        }

        [Fact]
        public async Task Success()
        {
            string tipoEmissao = "saida";
           
            string modelo = new Faker().Random.Number(50, 59).ToString(); //"55";
            string cnpj = GerarCNPJRandom.Execute();

            var ListaNotasMock = GerarDadosFaker( cnpj,  tipoEmissao,  modelo);

            var mockRepository = new Mock<INotaRepository>();

            mockRepository
              .Setup(i => i.ListaPorPeriodo(It.IsAny<DateTime>(), It.IsAny<DateTime>(), modelo))
              .Returns(ListaNotasMock);

            var useCase = new GetCurrentListaXMLNota(mockRepository.Object);

            var result = await useCase.ObterNotasUltimos7Dias(tipoEmissao, modelo);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].CNPJ.Should().Be(cnpj);
            result[0].FileName.Should().Contain(".xml");
        }

        [Fact]
        public async Task ObterNotasUltimos7DiasDeveRetornarListaVaziaQuandoRepositoryRetornaNull()
        {
            string modelo = "55";
            string tipoEmissao = "saida";
            string cnpj = GerarCNPJRandom.Execute();

            var mockRepository = new Mock<INotaRepository>();
            mockRepository
                .Setup(i => i.ListaPorPeriodo(It.IsAny<DateTime>(), It.IsAny<DateTime>(), modelo))
                .Returns(new List<ResponseNotaFiscal>());

            var useCase = new GetCurrentListaXMLNota(mockRepository.Object);

            var result = await useCase.ObterNotasUltimos7Dias(tipoEmissao, modelo);

            result.Should().BeEmpty(); 
        }

        [Fact]
        public async Task ObterNotasUltimos7DiasDeveLancarExcecaoQuandoRepositoryFalhar()
        {
            string tipoEmissao = "saida";
            string modelo = "55";

            var mockRepository = new Mock<INotaRepository>();
            mockRepository
                .Setup(i => i.ListaPorPeriodo(It.IsAny<DateTime>(), It.IsAny<DateTime>(), modelo))
                .Throws(new InvalidOperationException("Erro inesperado no banco de dados"));

            var useCase = new GetCurrentListaXMLNota(mockRepository.Object);

            Func<Task> act = async () => await useCase.ObterNotasUltimos7Dias(tipoEmissao, modelo);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Erro inesperado no banco de dados");
        }

    }
}
