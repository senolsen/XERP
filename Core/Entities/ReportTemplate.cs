using Core.Enums;

namespace Core.Entities
{
    public class ReportTemplate : BaseEntity // BaseEntity'den miras aldı
    {
        // Id ve CreatedAt silindi, çünkü BaseEntity'den geliyor
        public string Name { get; set; }
        public DocumentType DocumentType { get; set; }
        public bool IsDefault { get; set; }
        public byte[] LayoutData { get; set; }
    }
}