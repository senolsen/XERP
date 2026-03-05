namespace Core.Entities
{
    public class Teklif : BaseEntity
    {
        public string TeklifNo { get; set; }
        // public string MusteriAdi { get; set; } // BUNU SİLİYORUZ

        // YENİ EKLENEN (İlişki)
        public int MusteriId { get; set; }
        public virtual Musteri Musteri { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;
        public string Aciklama { get; set; }
        public decimal ToplamTutar { get; set; }

        public virtual ICollection<TeklifKalem> Kalemler { get; set; } = new List<TeklifKalem>();
    }
}