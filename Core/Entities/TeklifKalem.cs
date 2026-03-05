namespace Core.Entities
{
    public class TeklifKalem : BaseEntity
    {
        // public string UrunAdi { get; set; } // BUNU SİLİYORUZ

        // YENİ EKLENEN (İlişki)
        public int UrunId { get; set; }
        public virtual Urun Urun { get; set; }

        public int Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Tutar { get; set; }

        public int TeklifId { get; set; }
        public virtual Teklif Teklif { get; set; }
    }
}