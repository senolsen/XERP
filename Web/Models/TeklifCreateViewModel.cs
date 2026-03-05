namespace WebUI.Models
{
    public class TeklifCreateViewModel
    {
        public int MusteriId { get; set; }
        public string Aciklama { get; set; }
        public List<TeklifKalemViewModel> Kalemler { get; set; }
    }

    public class TeklifKalemViewModel
    {
        public int UrunId { get; set; }
        public int Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
    }
}