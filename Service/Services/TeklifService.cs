using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
    public class TeklifService : ITeklifService
    {
        private readonly IGenericRepository<Teklif> _teklifRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TeklifService(IGenericRepository<Teklif> teklifRepository, IUnitOfWork unitOfWork)
        {
            _teklifRepository = teklifRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Teklif>> GetAllAsync()
        {
            return await _teklifRepository.GetAll().ToListAsync();
        }

        public async Task<string> GetNextTeklifNoAsync()
        {
            string prefix = $"TKL-{DateTime.Now:yyyyMMdd}-";
            var sonTeklif = await _teklifRepository
                .Where(x => x.TeklifNo != null && x.TeklifNo.StartsWith(prefix))
                .OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            int siradakiNo = 1;
            if (sonTeklif != null)
            {
                string sonNoStr = sonTeklif.TeklifNo.Substring(prefix.Length);
                if (int.TryParse(sonNoStr, out int sonNo)) siradakiNo = sonNo + 1;
            }
            return $"{prefix}{siradakiNo:D4}";
        }

        public async Task<Teklif> TeklifOlusturAsync(int musteriId, string aciklama, DateTime? baslangic, DateTime? bitis, string paraBirimi, decimal kur, List<TeklifKalem> kalemler)
        {
            decimal araToplam = 0;
            decimal toplamIndirim = 0;
            decimal toplamKdv = 0;
            decimal belgeKur = kur == 0 ? 1 : kur; // Belge (Dip Toplam) Kuru

            foreach (var k in kalemler)
            {
                // 1. Satırı Kendi Döviziyle Hesapla
                decimal satirBrut = k.Miktar * k.BirimFiyat;
                k.IndirimTutari = satirBrut * (k.IndirimOrani / 100m);
                decimal satirNet = satirBrut - k.IndirimTutari;
                k.KdvTutari = satirNet * (k.KdvOrani / 100m);
                k.ToplamTutar = satirNet + k.KdvTutari;

                // 2. Satırı Önce TL'ye (Base) Çevir, Sonra İstenen Dip Toplam Dövize Çevir
                decimal tlBrut = satirBrut * k.Kur;
                decimal tlIndirim = k.IndirimTutari * k.Kur;
                decimal tlKdv = k.KdvTutari * k.Kur;

                // Ana toplamlara Belge Dövizi cinsinden ekle
                araToplam += (tlBrut / belgeKur);
                toplamIndirim += (tlIndirim / belgeKur);
                toplamKdv += (tlKdv / belgeKur);
            }

            var teklif = new Teklif
            {
                TeklifNo = await GetNextTeklifNoAsync(),
                Tarih = DateTime.Now,
                BaslangicTarihi = baslangic,
                BitisTarihi = bitis,
                ParaBirimi = paraBirimi,
                Kur = kur,
                MusteriId = musteriId,
                Aciklama = aciklama,
                AraToplam = araToplam,
                ToplamIndirim = toplamIndirim,
                ToplamKdv = toplamKdv,
                ToplamTutar = (araToplam - toplamIndirim) + toplamKdv,
                Kalemler = kalemler
            };

            await _teklifRepository.AddAsync(teklif);
            await _unitOfWork.CommitAsync();

            return teklif;
        }
    }
}