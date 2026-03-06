namespace Core.Entities
{
    public class ParaBirimi : BaseEntity
    {
        public string Kod { get; set; }
        public string Ad { get; set; }
        public string Sembol { get; set; }
        public bool IsDefault { get; set; }

        // YENİ: İlişkisel Bağlantı (Bir para biriminin birden çok günlük kuru olabilir)
        public virtual ICollection<DovizKuru> Kurlar { get; set; } = new List<DovizKuru>();
    }
}