namespace RedisMultiKeyTest.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public string? UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string? UsuarioUltimaAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}
