namespace RedisMultiKeyTest.Entities
{
    public class Pessoa : EntityBase
    {
        public string? Nome { get; set; }
        
        public IEnumerable<Documento>? Documentos { get; set; }
        
        public DateTime DataNascimento { get; set; }

        public IEnumerable<Endereco>? Enderecos { get; set; }
    }

    public class Documento
    {
        public string? Tipo { get; set; }
        public string? Valor { get; set; }
    }
}
