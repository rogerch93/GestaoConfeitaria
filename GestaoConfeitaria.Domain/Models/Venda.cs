namespace GestaoConfeitaria.Domain.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public DateTime? DataExclusao { get; set; } = DateTime.UtcNow;
        public decimal ValorTotal { get; set; }
        public int QuantidadeBolos { get; set; }
        public string FormaPagamento { get; set; } = "";
        public string Usuario { get; set; } = "root";

        // Construtor protegido para EF Core
        protected Venda() { }

        // Construtor para uso na aplicação
        public Venda(decimal valorTotal, int quantidadeBolos, string formaPagamento, string usuario)
        {
            if (valorTotal < 0)
                throw new ArgumentException("Valor total não pode ser negativo.", nameof(valorTotal));

            if (quantidadeBolos < 1)
                throw new ArgumentException("Quantidade de bolos deve ser no mínimo 1.", nameof(quantidadeBolos));

            if (string.IsNullOrWhiteSpace(formaPagamento))
                throw new ArgumentException("Forma de pagamento é obrigatória.", nameof(formaPagamento));

            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Usuário é obrigatório.", nameof(usuario));

            Data = DateTime.UtcNow;
            ValorTotal = valorTotal;
            QuantidadeBolos = quantidadeBolos;
            FormaPagamento = formaPagamento.Trim();
            Usuario = usuario.Trim();
        }
    }
}
