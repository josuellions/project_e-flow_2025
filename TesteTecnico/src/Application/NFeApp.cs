using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TesteTecnico.src.Contracts;
using TesteTecnico.src.Controllers;
using TesteTecnico.src.Repositories.DataAccess;
using TesteTecnico.src.UseCases.GerarXMLNota.GetCurrent;
using TesteTecnico.src.UseCases.GerarXMLNota.PostCurrent;
using TesteTecnico.src.Utils;
using TesteTecnico.src.Views;

public static class NFeApp
{
    /// <summary>
    /// Responsável por centralizar e executar as operações relacionadas ao processamento e gerenciamento de NF-e na aplicação.
    /// </summary>
    /// <param name="args"></param>
    /// <param name="nome"></param>
    /// <returns></returns>

    private static string? ObterParametro(string[] args, string nome)
    {
        var index = Array.IndexOf(args, nome);
        return (index != -1 && index + 1 < args.Length) ? args[index + 1] : null;
    }

    private static void ValidarParametros(string? modelo, string? tipo)
    {
        if (modelo == null || tipo == null)
        {
            Console.WriteLine("Erro: Para gerar, informe modelo e tipo.");
            return;
        }
        return;
    }


    public static async Task GerarXMLAsync(string[] args)
    {
        Console.WriteLine("Iniciando emissão de NFe...");
        var host = CriarHostComDI();
        var controller = host.Services.GetRequiredService<NotaController>();
        try
        {
            string? modelo = ObterParametro(args, "modelo");
            string? tipo = ObterParametro(args, "tipo");

            ValidarParametros(modelo, tipo);

            //var xml = controller.GerarXmlNota("saida", "12345678000195", "55");
            var modeloNota = modelo;//"55";
            string xml = string.Empty;

            int QTD_GERAR_XML = 1; //1 a 999
            
            for (int i = 0; i < QTD_GERAR_XML; i++)
            {
                var cnpjRandom = GerarCNPJRandom.Execute();
                xml = controller.GerarXmlNota("saida", modeloNota!, cnpjRandom);
            }

            Console.WriteLine("XML gerado com sucesso.");
            Console.WriteLine($"Arquivo: {xml}");

        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao gerar XML: " + ex.Message);
        }

        await host.StopAsync();
    }

    public static Task ListarXMLAsync(string[] args)
    {
        Console.WriteLine("Lista XML dos últimos 7 dias...");
        string? modelo = ObterParametro(args, "modelo");
        string? tipo = ObterParametro(args, "tipo");

        ValidarParametros(modelo, tipo);

        var host = CriarHostComDI();
        var controller = host.Services.GetRequiredService<NotaController>();
        var result = controller.ListaXmlNota(tipo!, modelo!);

        if (result.Count > 0)
        {
            GerarViewNotas.Lista(result);
        }
        else
        {
            Console.WriteLine("Nenhum XML encontrado");
        }

        return Task.CompletedTask;
    }

    private static IHost CriarHostComDI()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddTransient<NotaController>();
                services.AddTransient<PostCurrentGerarXMLNota>();
                services.AddTransient<GetCurrentListaXMLNota>();
                services.AddScoped<INotaRepository, NotaRepository>();
            })
            .Build();
    }
}
