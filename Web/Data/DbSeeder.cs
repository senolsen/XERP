using Microsoft.AspNetCore.Identity;
using Core.Entities;
using Core.Enums;
using Data.Context;
using System.Text;

namespace WebUI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // Identity yöneticilerini (Manager) ve Veritabanı Context'ini servisten çekiyoruz
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. Rolleri Tanımla
            string adminRoleName = "Admin";
            string userRoleName = "User";

            // Eğer "Admin" rolü yoksa oluştur
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = adminRoleName, Description = "Sistem Yöneticisi" });
            }

            // Eğer "User" rolü yoksa oluştur
            if (!await roleManager.RoleExistsAsync(userRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = userRoleName, Description = "Standart Kullanıcı" });
            }

            // 2. Admin Kullanıcısını Kontrol Et ve Oluştur
            string adminEmail = "admin@xerp.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null) // Kullanıcı yoksa yeni oluştur
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail, // Giriş yaparken e-posta kullanılacak
                    Email = adminEmail,
                    FirstName = "Sistem",
                    LastName = "Yöneticisi",
                    EmailConfirmed = true // E-posta onayını direkt true yapıyoruz ki onaysız hesaba takılmasın
                };

                // Kullanıcıyı belirlediğimiz şifre ile kaydediyoruz
                var result = await userManager.CreateAsync(adminUser, "Admin123*");

                if (result.Succeeded)
                {
                    // Oluşturulan kullanıcıya "Admin" rolünü ata
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                }
            }

            // 3. Varsayılan DevExpress Teklif Şablonunu Oluştur
            await SeedDefaultReportTemplatesAsync(dbContext);
        }

        private static async Task SeedDefaultReportTemplatesAsync(ApplicationDbContext dbContext)
        {
            // Eğer sistemde hiç Teklif şablonu yoksa ekle
            if (!dbContext.Set<ReportTemplate>().Any(t => t.DocumentType == DocumentType.Teklif))
            {
                // Profesyonel DevExpress REPX (XML) Tasarımı (Toplamlar Düzeltilmiş Hali)
                string repxTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
<XtraReportsLayoutSerializer SerializerVersion=""23.2.4.0"" Ref=""0"" ControlType=""DevExpress.XtraReports.UI.XtraReport, DevExpress.XtraReports.v23.2, Version=23.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"" Name=""TeklifRaporu"" Margins=""30, 30, 40, 40"" PageWidth=""827"" PageHeight=""1169"" PaperKind=""A4"" Font=""Arial, 9.7pt"" DataSource=""#Ref-0"">
  <Bands>
    <Item1 Ref=""1"" ControlType=""TopMarginBand"" Name=""TopMargin"" HeightF=""40"" />
    <Item2 Ref=""2"" ControlType=""BottomMarginBand"" Name=""BottomMargin"" HeightF=""40"" />
    <Item3 Ref=""3"" ControlType=""DetailBand"" Name=""Detail"" HeightF=""160"">
      <Controls>
        <Item1 Ref=""4"" ControlType=""XRLabel"" Name=""lblTeklifTitle"" Text=""TEKLİF"" TextAlignment=""MiddleRight"" Font=""Segoe UI, 26pt, style=Bold"" ForeColor=""41, 128, 185"" LocationFloat=""447, 0"" SizeF=""320, 45"" />
        <Item2 Ref=""5"" ControlType=""XRLabel"" Name=""lblFirmaAdi"" Text=""X-ERP YAZILIM A.Ş."" Font=""Segoe UI, 16pt, style=Bold"" ForeColor=""41, 128, 185"" LocationFloat=""0, 0"" SizeF=""350, 30"" />
        <Item3 Ref=""6"" ControlType=""XRLabel"" Name=""lblFirmaSlogan"" Text=""Kurumsal Teknoloji Çözümleri"" Font=""Segoe UI, 9pt"" ForeColor=""127, 140, 141"" LocationFloat=""0, 30"" SizeF=""350, 18"" />
        <Item4 Ref=""7"" ControlType=""XRLabel"" Name=""lblMusteriHeader"" Text=""SAYIN / MÜŞTERİ"" Font=""Segoe UI, 9pt, style=Bold"" ForeColor=""127, 140, 141"" LocationFloat=""0, 70"" SizeF=""350, 18"" />
        <Item5 Ref=""8"" ControlType=""XRLabel"" Name=""lblMusteriUnvan"" ExpressionBindings=""[Item1]"" Font=""Segoe UI, 12pt, style=Bold"" LocationFloat=""0, 88"" SizeF=""400, 23"">
          <ExpressionBindings>
            <Item1 Ref=""9"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[MusteriUnvan]"" />
          </ExpressionBindings>
        </Item5>
        <Item6 Ref=""10"" ControlType=""XRLabel"" Name=""lblMusteriVergi"" ExpressionBindings=""[Item1]"" Font=""Segoe UI, 10pt"" LocationFloat=""0, 111"" SizeF=""400, 23"">
          <ExpressionBindings>
            <Item1 Ref=""11"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[MusteriVergiDairesi] + ' VD. / ' + [MusteriVergiNo]"" />
          </ExpressionBindings>
        </Item6>
        <Item7 Ref=""12"" ControlType=""XRLabel"" Name=""lblTeklifNoLabel"" Text=""Teklif No:"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""127, 140, 141"" TextAlignment=""MiddleRight"" LocationFloat=""500, 70"" SizeF=""100, 23"" />
        <Item8 Ref=""13"" ControlType=""XRLabel"" Name=""lblTeklifNoVal"" ExpressionBindings=""[Item1]"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""41, 128, 185"" TextAlignment=""MiddleRight"" LocationFloat=""600, 70"" SizeF=""167, 23"">
          <ExpressionBindings>
            <Item1 Ref=""14"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[TeklifNo]"" />
          </ExpressionBindings>
        </Item8>
        <Item9 Ref=""15"" ControlType=""XRLabel"" Name=""lblTarihLabel"" Text=""Tarih:"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""127, 140, 141"" TextAlignment=""MiddleRight"" LocationFloat=""500, 93"" SizeF=""100, 23"" />
        <Item10 Ref=""16"" ControlType=""XRLabel"" Name=""lblTarihVal"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:dd.MM.yyyy}"" Font=""Segoe UI, 10pt"" TextAlignment=""MiddleRight"" LocationFloat=""600, 93"" SizeF=""167, 23"">
          <ExpressionBindings>
            <Item1 Ref=""17"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[Tarih]"" />
          </ExpressionBindings>
        </Item10>
        <Item11 Ref=""18"" ControlType=""XRLabel"" Name=""lblGecerlilikLabel"" Text=""Geçerlilik:"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""127, 140, 141"" TextAlignment=""MiddleRight"" LocationFloat=""500, 116"" SizeF=""100, 23"" />
        <Item12 Ref=""19"" ControlType=""XRLabel"" Name=""lblGecerlilikVal"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:dd.MM.yyyy}"" Font=""Segoe UI, 10pt"" TextAlignment=""MiddleRight"" LocationFloat=""600, 116"" SizeF=""167, 23"">
          <ExpressionBindings>
            <Item1 Ref=""20"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[BitisTarihi]"" />
          </ExpressionBindings>
        </Item12>
      </Controls>
    </Item3>
    <Item4 Ref=""21"" ControlType=""DetailReportBand"" Name=""DetailReport"" DataMember=""Kalemler"" DataSource=""#Ref-0"">
      <Bands>
        <Item1 Ref=""22"" ControlType=""GroupHeaderBand"" Name=""GroupHeader1"" HeightF=""35"">
          <Controls>
            <Item1 Ref=""23"" ControlType=""XRTable"" Name=""tableHeader"" LocationFloat=""0, 0"" SizeF=""767, 30"" BackColor=""236, 240, 241"" Font=""Segoe UI, 10pt, style=Bold"" TextAlignment=""MiddleCenter"">
              <Rows>
                <Item1 Ref=""24"" ControlType=""XRTableRow"" Name=""trHeader"" Weight=""1"">
                  <Cells>
                    <Item1 Ref=""25"" ControlType=""XRTableCell"" Name=""tcHeaderUrun"" Text=""Ürün / Hizmet Açıklaması"" TextAlignment=""MiddleLeft"" Padding=""10,0,0,0"" Weight=""1.5"" />
                    <Item2 Ref=""26"" ControlType=""XRTableCell"" Name=""tcHeaderMiktar"" Text=""Miktar"" Weight=""0.3"" />
                    <Item3 Ref=""27"" ControlType=""XRTableCell"" Name=""tcHeaderFiyat"" Text=""Birim Fiyat"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Weight=""0.6"" />
                    <Item4 Ref=""28"" ControlType=""XRTableCell"" Name=""tcHeaderToplam"" Text=""Toplam Tutar"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Weight=""0.6"" />
                  </Cells>
                </Item1>
              </Rows>
            </Item1>
          </Controls>
        </Item1>
        <Item2 Ref=""29"" ControlType=""DetailBand"" Name=""Detail1"" HeightF=""30"">
          <Controls>
            <Item1 Ref=""30"" ControlType=""XRTable"" Name=""tableDetay"" LocationFloat=""0, 0"" SizeF=""767, 30"" Borders=""Bottom"" BorderColor=""236, 240, 241"" Font=""Segoe UI, 10pt"" TextAlignment=""MiddleCenter"">
              <Rows>
                <Item1 Ref=""31"" ControlType=""XRTableRow"" Name=""trDetay"" Weight=""1"">
                  <Cells>
                    <Item1 Ref=""32"" ControlType=""XRTableCell"" Name=""tcDetayUrun"" ExpressionBindings=""[Item1]"" TextAlignment=""MiddleLeft"" Padding=""10,0,0,0"" Weight=""1.5"">
                      <ExpressionBindings>
                        <Item1 Ref=""33"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[UrunAdi]"" />
                      </ExpressionBindings>
                    </Item1>
                    <Item2 Ref=""34"" ControlType=""XRTableCell"" Name=""tcDetayMiktar"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:n0}"" Weight=""0.3"">
                      <ExpressionBindings>
                        <Item1 Ref=""35"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[Miktar]"" />
                      </ExpressionBindings>
                    </Item2>
                    <Item3 Ref=""36"" ControlType=""XRTableCell"" Name=""tcDetayFiyat"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:n2}"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Weight=""0.6"">
                      <ExpressionBindings>
                        <Item1 Ref=""37"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[BirimFiyat]"" />
                      </ExpressionBindings>
                    </Item3>
                    <Item4 Ref=""38"" ControlType=""XRTableCell"" Name=""tcDetayToplam"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:n2}"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""41, 128, 185"" Weight=""0.6"">
                      <ExpressionBindings>
                        <Item1 Ref=""39"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[ToplamFiyat]"" />
                      </ExpressionBindings>
                    </Item4>
                  </Cells>
                </Item1>
              </Rows>
            </Item1>
          </Controls>
        </Item2>
      </Bands>
    </Item4>
    <Item5 Ref=""40"" ControlType=""ReportFooterBand"" Name=""ReportFooter"" HeightF=""170"">
      <Controls>
        <Item1 Ref=""41"" ControlType=""XRLabel"" Name=""lblBankaHeader"" Text=""Banka Hesap Bilgilerimiz:"" Font=""Segoe UI, 9pt, style=Bold"" LocationFloat=""0, 15"" SizeF=""350, 18"" />
        <Item2 Ref=""42"" ControlType=""XRLabel"" Name=""lblBankaDetay"" Text=""Ziraat Bankası A.Ş.&#xD;&#xA;Alıcı: X-ERP YAZILIM A.Ş.&#xD;&#xA;IBAN: TR90 0000 0000 0000 0000 0000 00"" Multiline=""true"" Font=""Segoe UI, 9pt"" ForeColor=""127, 140, 141"" LocationFloat=""0, 35"" SizeF=""350, 50"" />
        <Item3 Ref=""43"" ControlType=""XRTable"" Name=""tableToplamlar"" LocationFloat=""467, 15"" SizeF=""300, 90"" Font=""Segoe UI, 10pt"">
          <Rows>
            <Item1 Ref=""44"" ControlType=""XRTableRow"" Name=""trAraToplam"" Weight=""1"">
              <Cells>
                <Item1 Ref=""45"" ControlType=""XRTableCell"" Name=""tcAraL"" Text=""Ara Toplam:"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" ForeColor=""127, 140, 141"" Weight=""1"" />
                <Item2 Ref=""46"" ControlType=""XRTableCell"" Name=""tcAraV"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:n2} ₺"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Font=""Segoe UI, 10pt, style=Bold"" Weight=""1"">
                  <ExpressionBindings>
                    <Item1 Ref=""47"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[AraToplam]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item1>
            <Item2 Ref=""48"" ControlType=""XRTableRow"" Name=""trKdv"" Weight=""1"">
              <Cells>
                <Item1 Ref=""49"" ControlType=""XRTableCell"" Name=""tcKdvL"" Text=""Toplam KDV:"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" ForeColor=""127, 140, 141"" Weight=""1"" />
                <Item2 Ref=""50"" ControlType=""XRTableCell"" Name=""tcKdvV"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:n2} ₺"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Font=""Segoe UI, 10pt, style=Bold"" Weight=""1"">
                  <ExpressionBindings>
                    <Item1 Ref=""51"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[ToplamKdv]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item2>
            <Item3 Ref=""52"" ControlType=""XRTableRow"" Name=""trGenelToplam"" Weight=""1.5"">
              <Cells>
                <Item1 Ref=""53"" ControlType=""XRTableCell"" Name=""tcGenelL"" Text=""GENEL TOPLAM:"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Font=""Segoe UI, 12pt, style=Bold"" Borders=""Top"" BorderColor=""189, 195, 199"" ForeColor=""41, 128, 185"" Weight=""1"" />
                <Item2 Ref=""54"" ControlType=""XRTableCell"" Name=""tcGenelV"" ExpressionBindings=""[Item1]"" TextFormatString=""{0:n2} ₺"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Font=""Segoe UI, 12pt, style=Bold"" Borders=""Top"" BorderColor=""189, 195, 199"" ForeColor=""41, 128, 185"" Weight=""1"">
                  <ExpressionBindings>
                    <Item1 Ref=""55"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[ToplamTutar]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item3>
          </Rows>
        </Item3>
        <Item4 Ref=""56"" ControlType=""XRLabel"" Name=""lblImza1Title"" Text=""Teklifi Hazırlayan"" Font=""Segoe UI, 10pt, style=Bold"" TextAlignment=""TopCenter"" LocationFloat=""0, 110"" SizeF=""200, 23"" />
        <Item5 Ref=""57"" ControlType=""XRLabel"" Name=""lblImza1Val"" Text=""X-ERP YAZILIM A.Ş."" Font=""Segoe UI, 9pt"" TextAlignment=""TopCenter"" LocationFloat=""0, 133"" SizeF=""200, 23"" />
        <Item6 Ref=""58"" ControlType=""XRLabel"" Name=""lblImza2Title"" Text=""Müşteri Onayı"" Font=""Segoe UI, 10pt, style=Bold"" TextAlignment=""TopCenter"" LocationFloat=""220, 110"" SizeF=""200, 23"" />
        <Item7 Ref=""59"" ControlType=""XRLabel"" Name=""lblImza2Val"" Text=""Kaşe ve İmza"" Font=""Segoe UI, 9pt"" TextAlignment=""TopCenter"" LocationFloat=""220, 133"" SizeF=""200, 23"" />
      </Controls>
    </Item5>
  </Bands>
  <ComponentStorage>
    <Item1 Ref=""0"" ObjectType=""DevExpress.DataAccess.ObjectBinding.ObjectDataSource, DevExpress.DataAccess.v23.2"" Name=""objectDataSource1"" Base64=""PE9iamVjdERhdGFTb3VyY2UgTmFtZT0ib2JqZWN0RGF0YVNvdXJjZTEiPjxEYXRhU291cmNlIFR5cGU9IkNvcmUuRFRPcy5SZXBvcnRpbmcuVGVrbGlmUmFwb3JEVE8sIENvcmUsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsIiAvPjwvT2JqZWN0RGF0YVNvdXJjZT4="" />
  </ComponentStorage>
</XtraReportsLayoutSerializer>";

                // Yeni Şablon Nesnesi Oluşturuyoruz ve XML Stringini Byte[] Dizisine Çeviriyoruz
                var newTemplate = new ReportTemplate
                {
                    Name = "Kurumsal Teklif Şablonu",
                    DocumentType = DocumentType.Teklif,
                    IsDefault = true,
                    LayoutData = Encoding.UTF8.GetBytes(repxTemplate)
                };

                dbContext.Set<ReportTemplate>().Add(newTemplate);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}