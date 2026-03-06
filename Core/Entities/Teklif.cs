namespace Core.Entities
{
    public class Teklif : BaseEntity
    {
        public string TeklifNo { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        // YENİ: Para Birimi ve Kur
        public string ParaBirimi { get; set; } = "TRY";
        public decimal Kur { get; set; } = 1;

        public int MusteriId { get; set; }
        public virtual Musteri Musteri { get; set; }
        public string? Aciklama { get; set; }

        public decimal AraToplam { get; set; }
        public decimal ToplamIndirim { get; set; }
        public decimal ToplamKdv { get; set; }
        public decimal ToplamTutar { get; set; }

        public virtual ICollection<TeklifKalem> Kalemler { get; set; } = new List<TeklifKalem>();
    }
}