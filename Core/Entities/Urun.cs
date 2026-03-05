namespace Core.Entities
{
    public class Urun : BaseEntity
    {
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string? Birimi { get; set; } // Adet, Kg, Metre vb.
        public decimal GuncelFiyat { get; set; } // Ürünün varsayılan fiyatı
        public int KdvOrani { get; set; } = 20;
    }
}