namespace WebUI.Models
{
    public class TeklifCreateViewModel
    {
        public int MusteriId { get; set; }
        public string Aciklama { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string ParaBirimi { get; set; }
        public decimal Kur { get; set; }
        public List<TeklifKalemViewModel> Kalemler { get; set; } = new List<TeklifKalemViewModel>();
    }

    public class TeklifKalemViewModel
    {
        public int SiraNo { get; set; }
        public string UrunKodu { get; set; }
        public string Aciklama { get; set; }
        public int UrunId { get; set; }
        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public string ParaBirimi { get; set; } // YENİ
        public decimal Kur { get; set; }       // YENİ
        public int IndirimOrani { get; set; }
        public int KdvOrani { get; set; }
    }
}