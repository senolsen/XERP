namespace Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // Loglama ve Takip (Audit) Alanları
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; } // Hangi kullanıcı oluşturdu? (Kullanıcı ID'si veya Adı tutulabilir)

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; } // Kim güncelledi?

        // Durum (Status) Alanları
        public bool IsActive { get; set; } = true; // Kayıt aktif mi?
        public bool IsDeleted { get; set; } = false; // Silindi mi? (Soft Delete için)
    }
}