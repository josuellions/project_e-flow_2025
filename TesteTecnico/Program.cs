using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TesteTecnico.src.Controllers;
using TesteTecnico.src.UseCases.GerarXMLNota.PostCurrent;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Controller
                services.AddTransient<NotaController>();

                // UseCase ou Service injetado na controller
                services.AddTransient<PostCurrentGerarXMLNota>();

                // Se PostCurrentGerarXMLNota tiver outras dependências, registre aqui também
            })
            .Build();

        //dotnet run -- gerar  -- modelo 55 -- tipo saida .\TesteTecnico.csproj 
        //dotnet run -- listar  -- modelo 55 -- tipo saida .\TesteTecnico.csproj 

        var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        #if DEBUG
        // Verifica se está no ambiente de desenvolvimento e nenhum argumento foi passado

        if (isDev && args.Length == 0)
        {
            // Força argumento "gerar" ou "listar" para teste automático
            args = new[] { "gerar", "modelo", "55", "tipo", "saida" };
            //args = new[] { "listar", "modelo", "55", "tipo", "saida" };
        }
        #endif

        if (args.Length == 0)
        {
            Console.WriteLine("Informe um comando: gerar ou listar, modelo e tipo");
            
            #if DEBUG
            Console.ReadLine();
            #endif

            return;
        }

        var argumentos = args.Skip(1).ToArray();
        var comando = args[0].ToLower();
        var comandos = new Dictionary<string, Func<string[], Task>>(StringComparer.OrdinalIgnoreCase)
        {
            ["gerar"] = NFeApp.GerarXMLAsync,
            ["listar"] = NFeApp.ListarXMLAsync
        };

        if (comandos.TryGetValue(args[0], out var executar))
        {
            await executar(argumentos);
        }
        else
        {
            Console.WriteLine($">>{args[0]}: Comando inválido! Use 'gerar' ou 'listar'.");
        }

        #if DEBUG
        Console.WriteLine("\n[DEBUG] Pressione Enter para sair...");
        Console.ReadLine();
        #endif
    }
}
