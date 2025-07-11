namespace TesteTecnico.Communication.Responses
{
    public class ResponseNotaFiscal
    {
        public DateTime Date { get; set; }

        public string CNPJ { get; set; } = string.Empty;

        public string Chave { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;
    }
}
