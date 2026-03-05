using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;

namespace Service.Services
{
    public class TeklifService : ITeklifService
    {
        private readonly IGenericRepository<Teklif> _teklifRepository;
        private readonly IUnitOfWork _unitOfWork;

        // Dependency Injection ile Repository ve UnitOfWork'ü alıyoruz
        public TeklifService(IGenericRepository<Teklif> teklifRepository, IUnitOfWork unitOfWork)
        {
            _teklifRepository = teklifRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Teklif> TeklifOlusturAsync(int musteriId, string aciklama, List<TeklifKalem> kalemler)
        {
            var yeniTeklif = new Teklif
            {
                TeklifNo = "TKL-" + DateTime.Now.ToString("yyyyMMdd-HHmm"),
                MusteriId = musteriId,
                Aciklama = aciklama,
                Tarih = DateTime.Now,
                ToplamTutar = 0
            };

            foreach (var kalem in kalemler)
            {
                kalem.Tutar = kalem.Miktar * kalem.BirimFiyat;
                yeniTeklif.ToplamTutar += kalem.Tutar;
                yeniTeklif.Kalemler.Add(kalem);
            }

            // Veriyi Repositor'ye ekle
            await _teklifRepository.AddAsync(yeniTeklif);

            // İşlemi Veritabanına Yansıt (Commit)
            await _unitOfWork.CommitAsync();

            return yeniTeklif;
        }
    }
}