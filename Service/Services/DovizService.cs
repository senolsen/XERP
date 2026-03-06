using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Xml;

namespace Service.Services
{
    public class DovizService : IDovizService
    {
        private readonly IGenericRepository<DovizKuru> _dovizRepository;
        private readonly IGenericRepository<ParaBirimi> _paraBirimiRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DovizService(
            IGenericRepository<DovizKuru> dovizRepository,
            IGenericRepository<ParaBirimi> paraBirimiRepository,
            IUnitOfWork unitOfWork)
        {
            _dovizRepository = dovizRepository;
            _paraBirimiRepository = paraBirimiRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ParaBirimleriniTcmbdenSenkronizeEtAsync()
        {
            var mevcutSayi = await _paraBirimiRepository.GetAll().CountAsync();
            if (mevcutSayi > 0) return;

            await _paraBirimiRepository.AddAsync(new ParaBirimi
            {
                Kod = "TRY",
                Ad = "Türk Lirası",
                Sembol = "₺",
                IsDefault = true,
                IsActive = true
            });

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("https://www.tcmb.gov.tr/kurlar/today.xml");

                XmlNodeList currencyNodes = xmlDoc.SelectNodes("Tarih_Date/Currency");
                foreach (XmlNode node in currencyNodes)
                {
                    string kod = node.Attributes["Kod"].Value;
                    string ad = node.SelectSingleNode("Isim").InnerText;

                    string sembol = kod switch
                    {
                        "USD" => "$",
                        "EUR" => "€",
                        "GBP" => "£",
                        "CHF" => "CHF",
                        "RUB" => "₽",
                        _ => kod
                    };

                    bool isAktif = kod == "USD" || kod == "EUR" || kod == "GBP";

                    await _paraBirimiRepository.AddAsync(new ParaBirimi
                    {
                        Kod = kod,
                        Ad = ad,
                        Sembol = sembol,
                        IsDefault = false,
                        IsActive = isAktif
                    });
                }
                await _unitOfWork.CommitAsync();
            }
            catch { }
        }

        public async Task<List<DovizKuru>> GetGunlukKurlarAsync()
        {
            var bugun = DateTime.Today;

            // DİKKAT: Veritabanından çekerken ParaBirimi nesnesini de (Include ile) dahil ediyoruz
            var kurlar = await _dovizRepository
                .Where(x => x.Tarih.Date == bugun)
                .Include(x => x.ParaBirimi)
                .ToListAsync();

            if (kurlar.Any()) return kurlar;

            // DİKKAT: Artık sadece string Kod değil, nesnenin tamamını alıyoruz ki ID'sine ulaşabilelim
            var aktifBirimler = await _paraBirimiRepository
                .Where(x => x.IsActive && !x.IsDeleted && x.Kod != "TRY")
                .ToListAsync();

            if (!aktifBirimler.Any()) return new List<DovizKuru>();

            var yeniKurlar = new List<DovizKuru>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("https://www.tcmb.gov.tr/kurlar/today.xml");

                foreach (var pb in aktifBirimler)
                {
                    XmlNode node = xmlDoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{pb.Kod}']");
                    if (node != null)
                    {
                        decimal satis = decimal.Parse(node.SelectSingleNode("BanknoteSelling").InnerText, CultureInfo.InvariantCulture);
                        decimal alis = decimal.Parse(node.SelectSingleNode("BanknoteBuying").InnerText, CultureInfo.InvariantCulture);

                        // DİKKAT: Artık string değil, doğrudan ParaBirimiId (Foreign Key) ile kaydediyoruz
                        var kur = new DovizKuru { Tarih = bugun, ParaBirimiId = pb.Id, ParaBirimi = pb, Alis = alis, Satis = satis };
                        yeniKurlar.Add(kur);
                        await _dovizRepository.AddAsync(kur);
                    }
                }
                await _unitOfWork.CommitAsync();
            }
            catch { }

            return yeniKurlar;
        }
    }
}