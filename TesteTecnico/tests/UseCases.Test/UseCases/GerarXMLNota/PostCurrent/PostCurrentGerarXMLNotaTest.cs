using Moq;
using FluentAssertions;
using System.Xml.Linq;
using TesteTecnico.src.Contracts;
using TesteTecnico.src.UseCases.GerarXMLNota.PostCurrent;
using TesteTecnico.src.Utils;

namespace UseCases.Test.UseCases.GerarXMLNota.PostCurrent
{
    public class PostCurrentGerarXMLNotaTest
    {
        private class DadosFaker
        {
            public string Modelo { get; set; } = "55";
            public string TipoNota { get; set; } = "saida";
            public string Cnpj { get; set; } = GerarCNPJRandom.Execute();
            public string Chave { get; set; } = GerarCNPJRandom.Execute();
            public string FileName { get; set; } = "";
            public DateTime Date { get; set; } = DateTime.Now;

        }

        private static DadosFaker GerarDadosFaker()
        {
            int anoFim = 2025;
            int anoInicio = 2022;

            DateTime dateFim = DateTime.Now;
            DateTime dateInicio = dateFim.AddDays(-7);
            DateTime date = GerarDateRandom.GerarDataAleatoria(anoInicio, anoFim);

            string modelo = "55";
            string cnpj = GerarCNPJRandom.Execute();
            string chave = GerarChaveNFeRandom.Execute(date, cnpj, modelo);
            string fileName = $"{chave}.xml";

            return new DadosFaker
            {
                Cnpj = cnpj,
                Chave = chave,
                FileName = fileName,
                Date = date
            };
        }

        [Fact]
        public async Task Success()
        {
            var dataFaker = GerarDadosFaker();
            var expectedResult = dataFaker.FileName;

            var repositoryMock = new Mock<INotaRepository>();
            repositoryMock.Setup(r => r.Create(dataFaker.Cnpj, dataFaker.Modelo, "1", It.IsAny<XElement>()))
                          .Returns(expectedResult);

            var useCase = new PostCurrentGerarXMLNota(repositoryMock.Object);

            var result = await useCase.Execute(dataFaker.TipoNota, dataFaker.Modelo, dataFaker.Cnpj);

            result.Should().Be(expectedResult);
            result.Should().Contain(".xml");

        }

        [Fact]
        public async Task ExecuteDeveLancarExcecaoQuandoRepositoryFalhar()
        {
            var dataFaker = GerarDadosFaker();

            var repositoryMock = new Mock<INotaRepository>();
            repositoryMock
                .Setup(r => r.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<XElement>()))
                .Throws(new InvalidOperationException("Erro na criação de XML"));

            var useCase = new PostCurrentGerarXMLNota(repositoryMock.Object);

            Func<Task> act = async () => await useCase.Execute(dataFaker.TipoNota, dataFaker.Modelo, dataFaker.Cnpj);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Erro na criação de XML");
        }

        [Fact]
        public async Task ExecuteDeveRetornarNullQuandoRepositoryRetornarNull()
        {
            var dataFaker = GerarDadosFaker();

            var repositoryMock = new Mock<INotaRepository>();
            repositoryMock
                .Setup(r => r.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<XElement>()))
                .Returns(string.Empty);

            var useCase = new PostCurrentGerarXMLNota(repositoryMock.Object);

            var result = await useCase.Execute(dataFaker.TipoNota, dataFaker.Modelo, dataFaker.Cnpj);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteDeveRetornarVazioQuandoRepositoryRetornarStringVazia()
        {
            var dataFaker = GerarDadosFaker();

            var repositoryMock = new Mock<INotaRepository>();
            repositoryMock
                .Setup(r => r.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<XElement>()))
                .Returns(string.Empty);

            var useCase = new PostCurrentGerarXMLNota(repositoryMock.Object);

            var result = await useCase.Execute(dataFaker.TipoNota, dataFaker.Modelo, dataFaker.Cnpj);

            result.Should().BeEmpty();
        }
    }
}
