using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Core.Entities;

namespace Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // IHttpContextAccessor Dependency Injection ile alınıyor
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<ReportTemplate> ReportTemplates { get; set; }
        public DbSet<Teklif> Teklifler { get; set; }
        public DbSet<TeklifKalem> TeklifKalemleri { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<DovizKuru> DovizKurlari { get; set; }
        public DbSet<ParaBirimi> ParaBirimleri { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Urun>().Property(x => x.GuncelFiyat).HasColumnType("decimal(18,2)");
            builder.Entity<Teklif>().Property(x => x.ToplamTutar).HasColumnType("decimal(18,2)");
            builder.Entity<TeklifKalem>().Property(x => x.BirimFiyat).HasColumnType("decimal(18,2)");
            builder.Entity<TeklifKalem>().Property(x => x.KdvTutari).HasColumnType("decimal(18,2)");
        }

        // Senkron kaydetme işlemi için araya giriyoruz
        public override int SaveChanges()
        {
            SetBaseProperties();
            return base.SaveChanges();
        }

        // Asenkron kaydetme işlemi için araya giriyoruz
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        // Asıl sihrin gerçekleştiği metot
        private void SetBaseProperties()
        {
            // O anki işlemlerde (Ekleme/Güncelleme) olan BaseEntity'leri yakala
            var entries = ChangeTracker.Entries<BaseEntity>();

            // Sisteme giriş yapmış kullanıcının ID'sini (veya Email'ini) al
            // Eğer kimse giriş yapmamışsa (örn: arka plan işlemiyse) "Sistem" yaz.
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Sistem";

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.IsActive = true;
                    entry.Entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;
                    entry.Entity.UpdatedBy = userId;

                    // Güvenlik: Güncelleme işleminde CreatedAt ve CreatedBy alanlarının yanlışlıkla değişmesini engelliyoruz
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                }
            }
        }
    }
}