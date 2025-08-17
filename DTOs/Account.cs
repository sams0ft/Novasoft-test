namespace novasoft_technical_test.DTOs
{
    public class Account
    {
        public string CodCli { get; set; } = string.Empty;
        public string NomCli { get; set; } = string.Empty;
        public string NitCli { get; set; } = string.Empty;
        public string CodCiu { get; set; } = string.Empty;
        public string CodDep { get; set; } = string.Empty;
        public string CodPai { get; set; } = string.Empty;
        public string Di2Cli { get; set; } = string.Empty;
        public string Te1Cli { get; set; } = string.Empty;
        public int TipCli { get; set; } = 0;
        public DateTime FecIng { get; set; } = DateTime.MinValue;
        public string EMail { get; set; } = string.Empty;
        public string TipIde { get; set; } = string.Empty;
        public string Ap1Cli { get; set; } = string.Empty;
        public string Nom1Cli { get; set; } = string.Empty;
        public int TipPer { get; set; } = 0;
        public string EstCli { get; set; } = string.Empty;
        public string CodCliExtr { get; set; } = string.Empty;
        public string PagWeb { get; set; } = string.Empty;
    }
}
