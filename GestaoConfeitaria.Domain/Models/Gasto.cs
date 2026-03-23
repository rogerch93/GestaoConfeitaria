namespace GestaoConfeitaria.Domain.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public DateTime? DataExclusao { get; set; } = DateTime.UtcNow;
        public string Descricao { get; set; } = "";
        public decimal Valor { get; set; }
        public string Usuario { get; set; } = "root";

        // Construtor protegido para EF Core
        protected Gasto() { }

        // Construtor para uso na aplicação
        public Gasto(string descricao, decimal valor, string usuario)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));

            if (valor <= 0)
                throw new ArgumentException("Valor do gasto deve ser maior que zero.", nameof(valor));

            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Usuário é obrigatório.", nameof(usuario));

            Data = DateTime.UtcNow;
            Descricao = descricao.Trim();
            Valor = valor;
            Usuario = usuario.Trim();
        }
    }
}
