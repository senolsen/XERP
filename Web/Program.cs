using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Data.Context;
using WebUI.Data;

var builder = WebApplication.CreateBuilder(args);

// DÝKKAT EKLENEN KOD: DevExpress Veri Güvenliði Ayarý (Core katmanýndaki sýnýflarýmýza izin veriyoruz)
DevExpress.Utils.DeserializationSettings.RegisterTrustedAssembly(typeof(Core.DTOs.Reporting.TeklifRaporDTO).Assembly);

// 1. EF Core & SQL Server Baðlantýsý
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("XERP.Data"))); // Migrations Data katmanýnda tutulacak

// BUNU EKLÝYORUZ: Giriþ yapan kullanýcý bilgilerine her katmandan eriþebilmek için
builder.Services.AddHttpContextAccessor();

// 2. Identity Yapýlandýrmasý (Dinamik Rol ve Kullanýcý Yönetimi Ýçin)
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. DevExpress Servislerinin Eklenmesi
builder.Services.AddDevExpressControls();
builder.Services.AddControllersWithViews();

// Mimari Servis Kayýtlarý
builder.Services.AddScoped<Core.UnitOfWorks.IUnitOfWork, Data.UnitOfWorks.UnitOfWork>();
builder.Services.AddScoped(typeof(Core.Repositories.IGenericRepository<>), typeof(Data.Repositories.GenericRepository<>));
builder.Services.AddScoped<Core.Services.ITeklifService, Service.Services.TeklifService>();

builder.Services.ConfigureReportingServices(configurator => {
    configurator.ConfigureReportDesigner(designerConfigurator => {
        designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
    });
    configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
        viewerConfigurator.UseCachedReportSourceBuilder();
    });
});

// BUNU EKLÝYORUZ: DevExpress Storage (Service katmanýndan çaðýrýyoruz)
builder.Services.AddScoped<DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension, Service.Services.CustomReportStorageWebExtension>();

var app = builder.Build();

// 4. OTOMATÝK MÝGRASYON VE SEED DATA UYGULAMA
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // 1. Veritabanýný oluþturur ve bekleyen migrasyonlarý uygular
        context.Database.Migrate();

        // 2. Admin Rolünü ve Kullanýcýsýný otomatik oluþturur (Burayý ekledik)
        // DbSeeder sýnýfýný kullanabilmek için dosyanýn en üstüne "using XERP.Web.Data;" eklemeyi unutmayýn.
        await DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný oluþturulurken veya Seed iþlemi sýrasýnda hata oluþtu.");
    }
}

// DevExpress ve Standart Middleware'ler
app.UseDevExpressControls();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication her zaman Authorization'dan önce gelmelidir!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();