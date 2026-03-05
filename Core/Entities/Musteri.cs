namespace Core.Entities
{
    public class Musteri : BaseEntity
    {
        public string Unvan { get; set; } // Firma Adı veya Ad Soyad
        public string? VergiDairesi { get; set; }
        public string? VergiNo { get; set; }
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        public string? Adres { get; set; }
    }
}