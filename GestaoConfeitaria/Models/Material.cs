namespace GestaoConfeitaria.Models
{
    public class Material
    {
        public int Id { get; set; }
        public DateTime DataUso { get; set; } = DateTime.UtcNow;
        public int VendaId { get; set; }
        public string Nome { get; set; } = "";
        public decimal Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }
        public decimal CustoTotal => Quantidade * CustoUnitario;
    }
}
