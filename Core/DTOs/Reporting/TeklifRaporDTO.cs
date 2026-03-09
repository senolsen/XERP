namespace Core.DTOs.Reporting
{
    public class TeklifRaporDTO
    {
        public string TeklifNo { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        public string ParaBirimi { get; set; }
        public string Aciklama { get; set; }

        public decimal AraToplam { get; set; }
        public decimal ToplamIndirim { get; set; }
        public decimal ToplamKdv { get; set; }
        public decimal ToplamTutar { get; set; }

        public string MusteriUnvan { get; set; }
        public string MusteriVergiNo { get; set; }
        public string MusteriVergiDairesi { get; set; }

        public List<TeklifKalemRaporDTO> Kalemler { get; set; } = new();
    }

    public class TeklifKalemRaporDTO
    {
        public string UrunAdi { get; set; }
        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }

        public decimal IndirimOrani { get; set; }
        public decimal IndirimTutari { get; set; }

        public decimal KdvOrani { get; set; }
        public decimal KdvTutari { get; set; }

        public decimal ToplamFiyat { get; set; }
    }
}