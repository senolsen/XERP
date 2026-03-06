namespace Core.Entities
{
    public class DovizKuru : BaseEntity
    {
        public DateTime Tarih { get; set; }

        // YENİ: String yerine doğrudan ana tabloya ID ile bağlanıyoruz
        public int ParaBirimiId { get; set; }
        public virtual ParaBirimi ParaBirimi { get; set; }

        public decimal Alis { get; set; }
        public decimal Satis { get; set; }
    }
}