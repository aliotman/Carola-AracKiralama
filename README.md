# Carola — Araç Kiralama Yönetim Sistemi

Çok şubeli bir araç kiralama şirketi için geliştirilmiş, ASP.NET Core MVC tabanlı web uygulaması. Müşterilerin şube ve tarih seçerek müsait araçları listeleyip rezervasyon yapabildiği bir vitrin ile şirketin araç, müşteri ve rezervasyon süreçlerini yönettiği bir admin panelinden oluşuyor.

Projeyi **N-Tier (katmanlı) mimari**, **Repository Pattern** ve **FluentValidation** konularını uçtan uca uygulamak için geliştirdim.

![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-6.0.36-512BD4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![FluentValidation](https://img.shields.io/badge/FluentValidation-11.12-1F8ACB?style=flat-square)

---

## Ekran Görüntüleri

<img width="331" height="1074" alt="01-anasayfa" src="https://github.com/user-attachments/assets/ff5a0ba7-441f-47b6-b3f0-b36f6b1b3b80" />
<img width="487" height="752" alt="02-arac-listesi" src="https://github.com/user-attachments/assets/a0d9368a-d624-41f3-bc8f-51ed68e4212a" />
<img width="851" height="624" alt="03-rezervasyon" src="https://github.com/user-attachments/assets/9d318b47-f79f-46a4-a1d5-bdbbc6e226c4" />
<img width="770" height="570" alt="04-dogrulama" src="https://github.com/user-attachments/assets/d0fad66e-2b4d-496a-b659-e40ce1c14343" />
<img width="1899" height="900" alt="admin-login" src="https://github.com/user-attachments/assets/d18ed7b4-a200-4392-9727-bdc851cb1c6a" />
<img width="1899" height="899" alt="05-admin-dashboard" src="https://github.com/user-attachments/assets/36a2b6a0-b04c-4376-b932-eb913e3ea060" />
<img width="1626" height="571" alt="06-admin-rezervasyon" src="https://github.com/user-attachments/assets/ec2e7dee-0450-4587-8c86-f1594fe8f680" />

---
## Öne Çıkan Özellikler

### Müşteri Tarafı

- **Gerçek müsaitlik kontrolü** — Seçilen tarih aralığında rezerve edilmiş araçlar sonuçlardan çıkarılır.
- **Çoklu filtreleme ve sayfalama** — Şube, tarih, kategori, vites, fiyat aralığı ve markaya göre filtreleme.
- **Ehliyet fotoğrafından otomatik form doldurma (OCR)** — Yüklenen görsel Tesseract ile okunur; ad, soyad, TC kimlik, ehliyet no ve doğum tarihi düzenli ifadelerle ayıklanıp forma yazılır.
- **Şeffaf fiyat hesabı** — Gün sayısı × günlük ücret rezervasyon ekranında adım adım gösterilir.
- **Çift katmanlı doğrulama** — TC kimlik formatı, 18 yaş sınırı, tarih tutarlılığı ve araç müsaitliği hem istemcide hem sunucuda denetlenir.
- **Rezervasyon gizliliği** — Onay sayfası yalnızca rezervasyonu oluşturan kişiye açılır.

### Admin Paneli

- **Kontrol paneli** — Araç, müşteri, rezervasyon, şube ve ciro istatistikleri.
- **CRUD yönetimi** — Araç, marka, şube, slider ve müşteri kayıtları; hepsi FluentValidation ile korunuyor.
- **Rezervasyon onay akışı** — Onayda araç "kirada" işaretlenir, müşteriye kupon kodlu e-posta gönderilir.
- **Cookie tabanlı kimlik doğrulama** — Tüm yönetim sayfaları `[Authorize]` ile korunur.
- **Veri asgariliği** — Müşteri listesinde TC kimlik ve ehliyet numarası gösterilmez.

---

| Katman | Sorumluluk |
|---|---|
| **EntityLayer** | Veritabanı tablolarının karşılığı olan 7 sınıf |
| **DtoLayer** | Katmanlar arası veri paketleri; entity'yi sunum katmanına açmaz |
| **DataAccessLayer** | Tüm veritabanı sorguları |
| **BusinessLayer** | İş kuralları, servisler ve doğrulama sınıfları |
| **WebUI** | Controller, Razor view,  view components, Admin area |

---

## Teknoloji Yığını

| Teknoloji | Kullanım Amacı |
|---|---|
| **ASP.NET Core MVC** | Web uygulama çatısı |
| **Entity Framework Core** | ORM, Code-First migration |
| **MS SQL Server** | İlişkisel veritabanı |
| **FluentValidation** | Doğrulama kuralları |
| **AutoMapper** | Entity ↔ DTO dönüşümü |
| **MailKit** | SMTP üzerinden e-posta gönderimi |
| **Tesseract** | Ehliyet görselinden OCR ile metin okuma |

> Arayüz geliştirme ve kullanıcı deneyimi tasarımı süreçlerinde Claude AI gibi yapay zeka araçlarından faydalandım.
