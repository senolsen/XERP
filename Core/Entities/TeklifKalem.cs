namespace Core.Entities
{
    public class TeklifKalem : BaseEntity
    {
        public int TeklifId { get; set; }
        public virtual Teklif Teklif { get; set; }

        public int SiraNo { get; set; }
        public string? UrunKodu { get; set; }
        public string? Aciklama { get; set; }

        public int UrunId { get; set; }
        public virtual Urun Urun { get; set; }

        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }

        // YENİ: Kalem Bazlı Para Birimi ve Kur
        public string ParaBirimi { get; set; } = "TRY";
        public decimal Kur { get; set; } = 1;

        public int IndirimOrani { get; set; }
        public decimal IndirimTutari { get; set; }
        public int KdvOrani { get; set; }
        public decimal KdvTutari { get; set; }
        public decimal ToplamTutar { get; set; }
    }
}