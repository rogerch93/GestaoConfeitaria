namespace GestaoConfeitaria.Domain.Models
{
    public class Material
    {
        public int Id { get; set; }
        public DateTime DataUso { get; set; } = DateTime.UtcNow;
        public DateTime? DataExclusao { get; set; } = DateTime.UtcNow;
        public int VendaId { get; set; }
        public string Nome { get; set; } = "";
        public decimal Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }
        public decimal CustoTotal => Quantidade * CustoUnitario;

        // Construtor protegido obrigatório para o EF Core
        protected Material() { }

        // Construtor para uso na aplicação
        public Material(int vendaId, string nome, decimal quantidade, decimal custoUnitario)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do material é obrigatório.", nameof(nome));

            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantidade));

            if (custoUnitario < 0)
                throw new ArgumentException("Custo unitário não pode ser negativo.", nameof(custoUnitario));

            VendaId = vendaId;
            Nome = nome.Trim();
            Quantidade = quantidade;
            CustoUnitario = custoUnitario;
            DataUso = DateTime.UtcNow;
        }
    }
}
