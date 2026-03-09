using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string adminRoleName = "Admin";
            string userRoleName = "User";

            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = adminRoleName,
                    Description = "Sistem Yöneticisi"
                });
            }

            if (!await roleManager.RoleExistsAsync(userRoleName))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = userRoleName,
                    Description = "Standart Kullanıcı"
                });
            }

            string adminEmail = "admin@xerp.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Sistem",
                    LastName = "Yöneticisi",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123*");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                }
            }

            await SeedDefaultReportTemplatesAsync(dbContext);
        }

        private static async Task SeedDefaultReportTemplatesAsync(ApplicationDbContext dbContext)
        {
            string repxTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
<XtraReportsLayoutSerializer SerializerVersion=""23.2.4.0"" Ref=""0"" ControlType=""DevExpress.XtraReports.UI.XtraReport, DevExpress.XtraReports.v23.2, Version=23.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"" Name=""TeklifRaporuModernRenkli"" Margins=""30, 30, 35, 35"" PageWidth=""827"" PageHeight=""1169"" PaperKind=""A4"" Font=""Segoe UI, 9.5pt"" DataSource=""#Ref-0"">
  <Bands>
    <Item1 Ref=""1"" ControlType=""TopMarginBand"" Name=""TopMargin"" HeightF=""30"" />

    <Item2 Ref=""2"" ControlType=""BottomMarginBand"" Name=""BottomMargin"" HeightF=""35"">
      <Controls>
        <Item1 Ref=""3"" ControlType=""XRLine"" Name=""lineFooterTop"" ForeColor=""220, 226, 237"" LineWidth=""1"" LocationFloat=""0, 0"" SizeF=""767, 2"" />
        <Item2 Ref=""4"" ControlType=""XRLabel"" Name=""lblFooterLeft"" Text=""Bu teklif XERP tarafından oluşturulmuştur."" Font=""Segoe UI, 8.5pt"" ForeColor=""110, 122, 145"" LocationFloat=""0, 8"" SizeF=""350, 18"" />
        <Item3 Ref=""5"" ControlType=""XRPageInfo"" Name=""pageInfo1"" TextFormatString=""Sayfa {0} / {1}"" Font=""Segoe UI, 8.5pt"" ForeColor=""110, 122, 145"" TextAlignment=""MiddleRight"" LocationFloat=""617, 8"" SizeF=""150, 18"" />
      </Controls>
    </Item2>

    <Item3 Ref=""6"" ControlType=""DetailBand"" Name=""Detail"" HeightF=""0"" />

    <Item4 Ref=""7"" ControlType=""ReportHeaderBand"" Name=""ReportHeader"" HeightF=""235"">
      <Controls>
        <Item1 Ref=""8"" ControlType=""XRLabel"" Name=""lblLogoBadge"" Text=""X"" Font=""Segoe UI, 22pt, style=Bold"" ForeColor=""255, 255, 255"" BackColor=""33, 111, 237"" TextAlignment=""MiddleCenter"" LocationFloat=""0, 0"" SizeF=""52, 52"" />
        <Item2 Ref=""9"" ControlType=""XRLabel"" Name=""lblFirmaAdi"" Text=""XERP YAZILIM A.Ş."" Font=""Segoe UI, 17pt, style=Bold"" ForeColor=""24, 39, 75"" LocationFloat=""66, 3"" SizeF=""320, 28"" />
        <Item3 Ref=""10"" ControlType=""XRLabel"" Name=""lblFirmaAlt"" Text=""Kurumsal teklif ve süreç yönetimi"" Font=""Segoe UI, 9.3pt"" ForeColor=""110, 122, 145"" LocationFloat=""66, 31"" SizeF=""260, 18"" />

        <Item4 Ref=""11"" ControlType=""XRLabel"" Name=""lblTeklifTitle"" Text=""TEKLİF FORMU"" Font=""Segoe UI, 23pt, style=Bold"" ForeColor=""33, 111, 237"" TextAlignment=""MiddleRight"" LocationFloat=""487, 0"" SizeF=""280, 34"" />
        <Item5 Ref=""12"" ControlType=""XRLabel"" Name=""lblTeklifSub"" Text=""Profesyonel fiyatlandırma ve teklif özeti"" Font=""Segoe UI, 9pt"" ForeColor=""110, 122, 145"" TextAlignment=""MiddleRight"" LocationFloat=""487, 36"" SizeF=""280, 18"" />

        <Item6 Ref=""13"" ControlType=""XRTable"" Name=""tableBlueBar"" LocationFloat=""0, 66"" SizeF=""767, 10"" BackColor=""33, 111, 237"">
          <Rows>
            <Item1 Ref=""14"" ControlType=""XRTableRow"" Name=""rowBlueBar"" Weight=""1"">
              <Cells>
                <Item1 Ref=""15"" ControlType=""XRTableCell"" Name=""cellBlueBar"" Weight=""1"" />
              </Cells>
            </Item1>
          </Rows>
        </Item6>

        <Item7 Ref=""16"" ControlType=""XRLabel"" Name=""lblMusteriSection"" Text=""MÜŞTERİ BİLGİLERİ"" Font=""Segoe UI, 9.5pt, style=Bold"" ForeColor=""33, 111, 237"" LocationFloat=""0, 92"" SizeF=""180, 18"" />
        <Item8 Ref=""17"" ControlType=""XRLabel"" Name=""lblMusteriUnvan"" Font=""Segoe UI, 13pt, style=Bold"" ForeColor=""24, 39, 75"" LocationFloat=""0, 115"" SizeF=""390, 24"">
          <ExpressionBindings>
            <Item1 Ref=""18"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[MusteriUnvan]"" />
          </ExpressionBindings>
        </Item8>
        <Item9 Ref=""19"" ControlType=""XRLabel"" Name=""lblMusteriVergi"" Font=""Segoe UI, 9.5pt"" ForeColor=""84, 95, 115"" LocationFloat=""0, 141"" SizeF=""390, 20"">
          <ExpressionBindings>
            <Item1 Ref=""20"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[MusteriVergiDairesi] + ' / ' + [MusteriVergiNo]"" />
          </ExpressionBindings>
        </Item9>

        <Item10 Ref=""21"" ControlType=""XRLabel"" Name=""lblAciklamaTitle"" Text=""AÇIKLAMA"" Font=""Segoe UI, 9pt, style=Bold"" ForeColor=""33, 111, 237"" LocationFloat=""0, 171"" SizeF=""120, 18"" />
        <Item11 Ref=""22"" ControlType=""XRLabel"" Name=""lblAciklama"" Multiline=""true"" Font=""Segoe UI, 9.3pt"" ForeColor=""84, 95, 115"" LocationFloat=""0, 192"" SizeF=""390, 38"">
          <ExpressionBindings>
            <Item1 Ref=""23"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[Aciklama]"" />
          </ExpressionBindings>
        </Item11>

        <Item12 Ref=""24"" ControlType=""XRTable"" Name=""tableTeklifInfo"" LocationFloat=""447, 92"" SizeF=""320, 110"" BorderColor=""221, 227, 237"" Borders=""All"" Font=""Segoe UI, 9.3pt"">
          <Rows>
            <Item1 Ref=""25"" ControlType=""XRTableRow"" Name=""rowInfo1"" Weight=""1"">
              <Cells>
                <Item1 Ref=""26"" ControlType=""XRTableCell"" Name=""cellInfo1L"" Text=""Teklif No"" BackColor=""245, 248, 253"" ForeColor=""96, 108, 130"" Font=""Segoe UI, 9pt, style=Bold"" Padding=""10,0,0,0"" Weight=""0.95"" />
                <Item2 Ref=""27"" ControlType=""XRTableCell"" Name=""cellInfo1V"" ForeColor=""24, 39, 75"" Font=""Segoe UI, 9.5pt, style=Bold"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Weight=""1.15"">
                  <ExpressionBindings>
                    <Item1 Ref=""28"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[TeklifNo]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item1>

            <Item2 Ref=""29"" ControlType=""XRTableRow"" Name=""rowInfo2"" Weight=""1"">
              <Cells>
                <Item1 Ref=""30"" ControlType=""XRTableCell"" Name=""cellInfo2L"" Text=""Tarih"" BackColor=""245, 248, 253"" ForeColor=""96, 108, 130"" Font=""Segoe UI, 9pt, style=Bold"" Padding=""10,0,0,0"" Weight=""0.95"" />
                <Item2 Ref=""31"" ControlType=""XRTableCell"" Name=""cellInfo2V"" TextFormatString=""{0:dd.MM.yyyy}"" ForeColor=""24, 39, 75"" Font=""Segoe UI, 9.3pt"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Weight=""1.15"">
                  <ExpressionBindings>
                    <Item1 Ref=""32"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[Tarih]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item2>

            <Item3 Ref=""33"" ControlType=""XRTableRow"" Name=""rowInfo3"" Weight=""1"">
              <Cells>
                <Item1 Ref=""34"" ControlType=""XRTableCell"" Name=""cellInfo3L"" Text=""Başlangıç"" BackColor=""245, 248, 253"" ForeColor=""96, 108, 130"" Font=""Segoe UI, 9pt, style=Bold"" Padding=""10,0,0,0"" Weight=""0.95"" />
                <Item2 Ref=""35"" ControlType=""XRTableCell"" Name=""cellInfo3V"" TextFormatString=""{0:dd.MM.yyyy}"" ForeColor=""24, 39, 75"" Font=""Segoe UI, 9.3pt"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Weight=""1.15"">
                  <ExpressionBindings>
                    <Item1 Ref=""36"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[BaslangicTarihi]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item3>

            <Item4 Ref=""37"" ControlType=""XRTableRow"" Name=""rowInfo4"" Weight=""1"">
              <Cells>
                <Item1 Ref=""38"" ControlType=""XRTableCell"" Name=""cellInfo4L"" Text=""Geçerlilik"" BackColor=""245, 248, 253"" ForeColor=""96, 108, 130"" Font=""Segoe UI, 9pt, style=Bold"" Padding=""10,0,0,0"" Weight=""0.95"" />
                <Item2 Ref=""39"" ControlType=""XRTableCell"" Name=""cellInfo4V"" TextFormatString=""{0:dd.MM.yyyy}"" ForeColor=""24, 39, 75"" Font=""Segoe UI, 9.3pt"" TextAlignment=""MiddleRight"" Padding=""0,10,0,0"" Weight=""1.15"">
                  <ExpressionBindings>
                    <Item1 Ref=""40"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[BitisTarihi]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item4>
          </Rows>
        </Item12>
      </Controls>
    </Item4>

    <Item5 Ref=""41"" ControlType=""DetailReportBand"" Name=""DetailReport"" DataMember=""Kalemler"" DataSource=""#Ref-0"">
      <Bands>
        <Item1 Ref=""42"" ControlType=""GroupHeaderBand"" Name=""GroupHeader1"" HeightF=""36"">
          <Controls>
            <Item1 Ref=""43"" ControlType=""XRTable"" Name=""tableKalemHeader"" LocationFloat=""0, 0"" SizeF=""767, 34"" BackColor=""33, 111, 237"" ForeColor=""255, 255, 255"" Font=""Segoe UI, 10pt, style=Bold"" TextAlignment=""MiddleCenter"">
              <Rows>
                <Item1 Ref=""44"" ControlType=""XRTableRow"" Name=""rowKalemHeader"" Weight=""1"">
                  <Cells>
                    <Item1 Ref=""45"" ControlType=""XRTableCell"" Name=""cellHUrun"" Text=""Ürün / Hizmet"" TextAlignment=""MiddleLeft"" Padding=""12,0,0,0"" Weight=""1.75"" />
                    <Item2 Ref=""46"" ControlType=""XRTableCell"" Name=""cellHMiktar"" Text=""Miktar"" Weight=""0.35"" />
                    <Item3 Ref=""47"" ControlType=""XRTableCell"" Name=""cellHFiyat"" Text=""Birim Fiyat"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Weight=""0.55"" />
                    <Item4 Ref=""48"" ControlType=""XRTableCell"" Name=""cellHToplam"" Text=""Toplam"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Weight=""0.55"" />
                  </Cells>
                </Item1>
              </Rows>
            </Item1>
          </Controls>
        </Item1>

        <Item2 Ref=""49"" ControlType=""DetailBand"" Name=""Detail1"" HeightF=""30"">
          <Controls>
            <Item1 Ref=""50"" ControlType=""XRTable"" Name=""tableKalemDetay"" LocationFloat=""0, 0"" SizeF=""767, 30"" Borders=""Bottom"" BorderColor=""229, 234, 242"" Font=""Segoe UI, 9.4pt"" TextAlignment=""MiddleCenter"">
              <Rows>
                <Item1 Ref=""51"" ControlType=""XRTableRow"" Name=""rowKalemDetay"" Weight=""1"">
                  <Cells>
                    <Item1 Ref=""52"" ControlType=""XRTableCell"" Name=""cellDUrun"" TextAlignment=""MiddleLeft"" Padding=""12,0,0,0"" Weight=""1.75"">
                      <ExpressionBindings>
                        <Item1 Ref=""53"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[UrunAdi]"" />
                      </ExpressionBindings>
                    </Item1>
                    <Item2 Ref=""54"" ControlType=""XRTableCell"" Name=""cellDMiktar"" TextFormatString=""{0:n2}"" Weight=""0.35"">
                      <ExpressionBindings>
                        <Item1 Ref=""55"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[Miktar]"" />
                      </ExpressionBindings>
                    </Item2>
                    <Item3 Ref=""56"" ControlType=""XRTableCell"" Name=""cellDFiyat"" TextFormatString=""{0:n2}"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Weight=""0.55"">
                      <ExpressionBindings>
                        <Item1 Ref=""57"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[BirimFiyat]"" />
                      </ExpressionBindings>
                    </Item3>
                    <Item4 Ref=""58"" ControlType=""XRTableCell"" Name=""cellDToplam"" TextFormatString=""{0:n2}"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Font=""Segoe UI, 9.4pt, style=Bold"" ForeColor=""24, 39, 75"" Weight=""0.55"">
                      <ExpressionBindings>
                        <Item1 Ref=""59"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[ToplamFiyat]"" />
                      </ExpressionBindings>
                    </Item4>
                  </Cells>
                </Item1>
              </Rows>
            </Item1>
          </Controls>
        </Item2>
      </Bands>
    </Item5>

    <Item6 Ref=""60"" ControlType=""ReportFooterBand"" Name=""ReportFooter"" HeightF=""200"">
      <Controls>
        <Item1 Ref=""61"" ControlType=""XRLabel"" Name=""lblNotlarTitle"" Text=""Notlar"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""33, 111, 237"" LocationFloat=""0, 12"" SizeF=""120, 20"" />
        <Item2 Ref=""62"" ControlType=""XRLabel"" Name=""lblNotlarBody"" Text=""• Fiyatlara aksi belirtilmedikçe KDV dahil değildir.&#xD;&#xA;• Teklif belirtilen süre boyunca geçerlidir.&#xD;&#xA;• Onay sonrası uygulama ve teslim planı ayrıca paylaşılır."" Multiline=""true"" Font=""Segoe UI, 9pt"" ForeColor=""98, 109, 128"" LocationFloat=""0, 36"" SizeF=""360, 70"" />

        <Item3 Ref=""63"" ControlType=""XRLabel"" Name=""lblParaBirimiInfo"" Font=""Segoe UI, 9pt, style=Bold"" ForeColor=""33, 111, 237"" TextAlignment=""MiddleRight"" LocationFloat=""497, 10"" SizeF=""270, 18"">
          <ExpressionBindings>
            <Item1 Ref=""64"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""'Para Birimi: ' + [ParaBirimi]"" />
          </ExpressionBindings>
        </Item3>

        <Item4 Ref=""65"" ControlType=""XRTable"" Name=""tableToplamlar"" LocationFloat=""447, 36"" SizeF=""320, 108"" BorderColor=""221, 227, 237"" Borders=""All"" Font=""Segoe UI, 9.5pt"">
          <Rows>
            <Item1 Ref=""66"" ControlType=""XRTableRow"" Name=""rowTop1"" Weight=""1"">
              <Cells>
                <Item1 Ref=""67"" ControlType=""XRTableCell"" Name=""cellTop1L"" Text=""Ara Toplam"" BackColor=""245, 248, 253"" ForeColor=""96, 108, 130"" Font=""Segoe UI, 9pt, style=Bold"" Padding=""12,0,0,0"" Weight=""1"" />
                <Item2 Ref=""68"" ControlType=""XRTableCell"" Name=""cellTop1V"" TextFormatString=""{0:n2}"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Font=""Segoe UI, 9.5pt, style=Bold"" Weight=""1"">
                  <ExpressionBindings>
                    <Item1 Ref=""69"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[AraToplam]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item1>

            <Item2 Ref=""70"" ControlType=""XRTableRow"" Name=""rowTop2"" Weight=""1"">
              <Cells>
                <Item1 Ref=""71"" ControlType=""XRTableCell"" Name=""cellTop2L"" Text=""Toplam İndirim"" BackColor=""245, 248, 253"" ForeColor=""96, 108, 130"" Font=""Segoe UI, 9pt, style=Bold"" Padding=""12,0,0,0"" Weight=""1"" />
                <Item2 Ref=""72"" ControlType=""XRTableCell"" Name=""cellTop2V"" TextFormatString=""{0:n2}"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Font=""Segoe UI, 9.5pt, style=Bold"" Weight=""1"">
                  <ExpressionBindings>
                    <Item1 Ref=""73"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[ToplamIndirim]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item2>

            <Item3 Ref=""74"" ControlType=""XRTableRow"" Name=""rowTop3"" Weight=""1"">
              <Cells>
                <Item1 Ref=""75"" ControlType=""XRTableCell"" Name=""cellTop3L"" Text=""Toplam KDV"" BackColor=""245, 248, 253"" ForeColor=""96, 108, 130"" Font=""Segoe UI, 9pt, style=Bold"" Padding=""12,0,0,0"" Weight=""1"" />
                <Item2 Ref=""76"" ControlType=""XRTableCell"" Name=""cellTop3V"" TextFormatString=""{0:n2}"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Font=""Segoe UI, 9.5pt, style=Bold"" Weight=""1"">
                  <ExpressionBindings>
                    <Item1 Ref=""77"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[ToplamKdv]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item3>

            <Item4 Ref=""78"" ControlType=""XRTableRow"" Name=""rowTop4"" Weight=""1.15"">
              <Cells>
                <Item1 Ref=""79"" ControlType=""XRTableCell"" Name=""cellTop4L"" Text=""GENEL TOPLAM"" BackColor=""33, 111, 237"" ForeColor=""255, 255, 255"" Font=""Segoe UI, 10.5pt, style=Bold"" Padding=""12,0,0,0"" Weight=""1"" />
                <Item2 Ref=""80"" ControlType=""XRTableCell"" Name=""cellTop4V"" TextFormatString=""{0:n2}"" BackColor=""33, 111, 237"" ForeColor=""255, 255, 255"" TextAlignment=""MiddleRight"" Padding=""0,12,0,0"" Font=""Segoe UI, 10.5pt, style=Bold"" Weight=""1"">
                  <ExpressionBindings>
                    <Item1 Ref=""81"" EventName=""BeforePrint"" PropertyName=""Text"" Expression=""[ToplamTutar]"" />
                  </ExpressionBindings>
                </Item2>
              </Cells>
            </Item4>
          </Rows>
        </Item4>

        <Item5 Ref=""82"" ControlType=""XRLine"" Name=""lineSignTop"" ForeColor=""220, 226, 237"" LineWidth=""1"" LocationFloat=""0, 162"" SizeF=""767, 2"" />
        <Item6 Ref=""83"" ControlType=""XRLabel"" Name=""lblHazirlayanTitle"" Text=""Hazırlayan"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""24, 39, 75"" TextAlignment=""TopCenter"" LocationFloat=""20, 170"" SizeF=""220, 22"" />
        <Item7 Ref=""84"" ControlType=""XRLabel"" Name=""lblHazirlayanVal"" Text=""XERP Yazılım A.Ş."" Font=""Segoe UI, 9pt"" ForeColor=""98, 109, 128"" TextAlignment=""TopCenter"" LocationFloat=""20, 192"" SizeF=""220, 18"" />

        <Item8 Ref=""85"" ControlType=""XRLabel"" Name=""lblMusteriOnayTitle"" Text=""Müşteri Onayı"" Font=""Segoe UI, 10pt, style=Bold"" ForeColor=""24, 39, 75"" TextAlignment=""TopCenter"" LocationFloat=""527, 170"" SizeF=""220, 22"" />
        <Item9 Ref=""86"" ControlType=""XRLabel"" Name=""lblMusteriOnayVal"" Text=""Kaşe / İmza"" Font=""Segoe UI, 9pt"" ForeColor=""98, 109, 128"" TextAlignment=""TopCenter"" LocationFloat=""527, 192"" SizeF=""220, 18"" />
      </Controls>
    </Item6>
  </Bands>

  <ComponentStorage>
    <Item1 Ref=""87"" ObjectType=""DevExpress.DataAccess.ObjectBinding.ObjectDataSource, DevExpress.DataAccess.v23.2"" Name=""objectDataSource1"" Base64=""PE9iamVjdERhdGFTb3VyY2UgTmFtZT0ib2JqZWN0RGF0YVNvdXJjZTEiPjxEYXRhU291cmNlIFR5cGU9IkNvcmUuRFRPcy5SZXBvcnRpbmcuVGVrbGlmUmFwb3JEVE8sIENvcmUsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsIiAvPjwvT2JqZWN0RGF0YVNvdXJjZT4="" />
  </ComponentStorage>
</XtraReportsLayoutSerializer>";

            var existingDefaultTemplate = await dbContext.Set<ReportTemplate>()
                .FirstOrDefaultAsync(t => t.DocumentType == DocumentType.Teklif && t.IsDefault);

            if (existingDefaultTemplate == null)
            {
                var newTemplate = new ReportTemplate
                {
                    Name = "Modern Renkli Teklif Şablonu",
                    DocumentType = DocumentType.Teklif,
                    IsDefault = true,
                    LayoutData = Encoding.UTF8.GetBytes(repxTemplate)
                };

                await dbContext.Set<ReportTemplate>().AddAsync(newTemplate);
            }
            else
            {
                existingDefaultTemplate.Name = "Modern Renkli Teklif Şablonu";
                existingDefaultTemplate.LayoutData = Encoding.UTF8.GetBytes(repxTemplate);
                existingDefaultTemplate.IsDefault = true;
            }

            var otherTemplates = await dbContext.Set<ReportTemplate>()
                .Where(t => t.DocumentType == DocumentType.Teklif && !t.IsDefault)
                .ToListAsync();

            await dbContext.SaveChangesAsync();
        }
    }
}