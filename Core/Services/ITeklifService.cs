using Core.Entities;

namespace Core.Services
{
    // Özel iş kurallarımız (ViewModel'dan gelen veriyi kaydetme vs.) burada tanımlanır
    public interface ITeklifService
    {
        Task<IEnumerable<Teklif>> GetAllAsync();

        // YENİ: Sıradaki teklif numarasını hesaplayıp getirecek metot
        Task<string> GetNextTeklifNoAsync();

        // DİKKAT: Metot imzasına tarih parametreleri eklendi
        // DİKKAT: ParaBirimi ve Kur eklendi
        Task<Teklif> TeklifOlusturAsync(int musteriId, string aciklama, DateTime? baslangic, DateTime? bitis, string paraBirimi, decimal kur, List<TeklifKalem> kalemler);
    }
}