namespace GestaoConfeitaria.Shared.DTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public DateTime DataUso { get; set; }
        public DateTime DataExclusao { get; set; }
        public int VendaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }
        public decimal CustoTotal { get; set; }   // calculado
    }
}
