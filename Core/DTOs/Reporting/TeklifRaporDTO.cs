using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Reporting
{
    // 1. Ana Teklif DTO'muz (Müşteri bilgilerini de içine düzleştirdik/flatten yaptık)
    public class TeklifRaporDTO
    {
        public string TeklifNo { get; set; }
        public DateTime Tarih { get; set; }
        public decimal ToplamTutar { get; set; }

        // Müşteri Entity'sine gitmek yerine sadece raporda lazım olanları aldık
        public string MusteriUnvan { get; set; }
        public string MusteriVergiNo { get; set; }
        public string MusteriVergiDairesi { get; set; }

        // Alt Kalemler Listesi
        public List<TeklifKalemRaporDTO> Kalemler { get; set; } = new List<TeklifKalemRaporDTO>();
    }

    // 2. Teklifin İçindeki Satırlar (Kalemler) İçin DTO
    public class TeklifKalemRaporDTO
    {
        // Ürün Entity'sine gitmek yerine sadece adını aldık
        public string UrunAdi { get; set; }
        public decimal Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamFiyat { get; set; } // Miktar * BirimFiyat
    }
}
