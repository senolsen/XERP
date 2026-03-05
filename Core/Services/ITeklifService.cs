using Core.Entities;

namespace Core.Services
{
    // Özel iş kurallarımız (ViewModel'dan gelen veriyi kaydetme vs.) burada tanımlanır
    public interface ITeklifService
    {
        Task<Teklif> TeklifOlusturAsync(int musteriId, string aciklama, List<TeklifKalem> kalemler);
        // İleride buraya: Onayla(), Reddet(), PdfOlustur() gibi metotlar eklenecek.
    }
}