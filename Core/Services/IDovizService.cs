using Core.Entities;

namespace Core.Services
{
    public interface IDovizService
    {
        // YENİ: Sistem ilk kurulduğunda Para Birimi tablosunu dolduracak metot
        Task ParaBirimleriniTcmbdenSenkronizeEtAsync();

        Task<List<DovizKuru>> GetGunlukKurlarAsync();
    }
}