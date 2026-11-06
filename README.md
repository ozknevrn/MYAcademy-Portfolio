# Portfolyo Projesi — ASP.NET Core MVC

M&Y Yazılım Eğitim Akademi · 12. Dönem Gündüz Grubu · **Proje 01 – Portfolyo Case**

ASP.NET Core MVC (.NET 8), Entity Framework Core (Code First) ve SQL Server ile geliştirilmiş, **yönetim paneli (Admin)** ve **kullanıcı arayüzü (UI)** olmak üzere iki ekrandan oluşan kişisel portfolyo uygulaması. Admin panelinde yapılan her değişiklik sitede anında görünür.

---

## Kurulum

1. Projeyi klonlayın ve Visual Studio 2022 ile `MYAcademey-Portfollo.sln` dosyasını açın.
2. `Context/PortfolioContext.cs` içindeki bağlantı adresini kendi SQL Server'ınıza göre düzenleyin:
   ```csharp
   optionsBuilder.UseSqlServer(@"Server=SUNUCU_ADINIZ;Database=PortfolioDayDb;integrated security=true;Trust Server Certificate=True");
   ```
3. **Package Manager Console**'da veritabanını oluşturun / güncelleyin:
   ```
   update-database
   ```
4. Projeyi çalıştırın.
   - Site (UI): `https://localhost:xxxx/`
   - Yönetim paneli: `https://localhost:xxxx/Admin`

Yüklenen görseller `wwwroot/uploads/` klasörüne kaydedilir. Veritabanında sadece dosya yolu tutulur.

---

## Yapılan Geliştirmeler

### 01 · Temel modüller ve UI entegrasyonu

| Modül | Admin (Yönetim) | UI (Site) |
|---|---|---|
| **Ana Sayfa** | İsim, unvan, tanıtım, profil fotoğrafı, öne çıkan bilgiler (10+ yıl, 200K+ öğrenci). Yazı makinesi efektinde dönen unvanlar (ekle / aktif-pasif / sil) | Hero bölümü |
| **Hakkımda** | Tek kayıt, güncelleme odaklı ekran + fotoğraf yükleme | "Beni Tanıyın" bölümü |
| **Yeteneklerim** | Yetenek grubu (Skill) + yetenek (SkillItem) CRUD, 0-100 seviye | Grup kartları + seviye çubukları |
| **Deneyimlerim** | CRUD, arama, aktif/pasif | Zaman çizelgesi (sadece aktifler) |
| **Projelerim** | CRUD, kategori, görsel, teknolojiler, aktif/pasif | Proje kartları + kategori filtresi |
| **Kategoriler** | CRUD, kategori başına proje sayısı | Proje filtre butonları |
| **Eğitim** | CRUD, aktif/pasif | "Eğitim Hayatım" bölümü |
| **İletişim Bilgileri** | CRUD (e-posta, telefon, konum) | İletişim bölümü |
| **Sosyal Medya** | CRUD (platform, link, ikon) | Hero, iletişim ve footer bağlantıları |
| **Gelen Mesajlar** | Bkz. madde 04 | Çalışan iletişim formu |

Tüm admin menü linkleri çalışır durumdadır. Açık olan sayfanın menüsü otomatik olarak vurgulanır.

### 02 · İlişkili tablolar ve veri doğrulama

- **Category – Project (bire-çok):** `Project.CategoryId` (Foreign Key) + `Project.Category` / `Category.Projects` (Navigation Property).
- **Dropdown:** Proje ekleme/güncelleme ekranında kategoriler veritabanından `SelectList` ile doldurulur, `asp-items` ile gösterilir. Aynı yapı yetenek → yetenek grubu seçiminde de kullanıldı.
- **Include:** Proje listesinde kategori adı `Include(x => x.Category)` ile getirilir.
- **Kategori silme koruması:** Projesi olan bir kategori silinmek istenirse, cascade delete ile projelerin de silinmemesi için işlem engellenir ve uyarı gösterilir.
- **Data Annotations:** `[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`, `[Url]`, `[Phone]`. Hata mesajları Türkçedir.
- **Sunucu doğrulaması:** Her kayıt işleminde `ModelState.IsValid` kontrol edilir. Hata varsa form, girilen bilgilerle ve alan bazlı hata mesajlarıyla (`asp-validation-for`) yeniden gösterilir. Tarayıcı tarafında da jQuery Unobtrusive Validation çalışır.
- **Görsel yükleme (IFormFile):** `Services/ImageUploadService.cs`
  - Sadece `.jpg`, `.jpeg`, `.png`, `.webp` dosyalarına izin verilir, en fazla 2 MB.
  - Dosya adı `Guid` ile güvenli şekilde üretilir. Kullanıcının dosya adı kullanılmaz.
  - Veritabanında sadece yol tutulur (`/uploads/projects/xxx.jpg`).
  - Görsel değiştirildiğinde ya da proje silindiğinde eski dosya diskten de silinir.
  - Aynı servis Proje, Ana Sayfa ve Hakkımda ekranlarında ortak kullanılır (Dependency Injection).

### 03 · Kullanıcı deneyimi ve içerik kontrolü

- **Silme onayı:** Tüm silme işlemleri **SweetAlert2** onay penceresi açar. `class="js-confirm-delete"` eklenen her link için tek bir ortak script çalışır.
- **Bildirimler:** Kayıt, güncelleme ve silme işlemlerinden sonra `TempData` ile gelen mesaj sağ üstte SweetAlert2 toast olarak gösterilir.
- **Aktif / Pasif:** Projeler, deneyimler, eğitimler ve dönen unvanlar tek tıkla aktif ya da pasif yapılabilir. Pasif kayıtlar admin panelinde görünür ama sitede **listelenmez**.
- **Proje listesi:** Kategori filtresi, durum filtresi, arama ve sıralama (**en yeni**, en eski, A-Z). Sitede projeler en yeniden eskiye sıralanır ve kategoriye göre filtrelenebilir.

### 04 · Gelen Kutusu: gelişmiş mesaj yönetimi

- Sitedeki iletişim formu, sayfa yenilenmeden (`fetch`) `DefaultController/SendMessage` adresine gönderilir. Mesaj veritabanına kaydedilir. `[ValidateAntiForgeryToken]` ile korunur.
- Zorunlu alan ve e-posta doğrulaması sunucuda yapılır. Hatalar JSON olarak alan bazlı döner ve ilgili kutunun altında gösterilir.
- Mesajlar en yeniden eskiye listelenir. Gönderen, konu veya e-postaya göre arama yapılabilir, okunmuş/okunmamış filtresi vardır.
- Mesaj detayı **Bootstrap Modal** içinde, sayfa yenilenmeden açılır (`GetMessageDetail` JSON). Açılan mesaj otomatik olarak **okundu** olur, satır ve sayaçlar anında güncellenir.
- Okunmamış mesajlar tabloda yeşil şerit, nokta ve kalın yazıyla ayrılır. Okunmamış mesaj sayısı **sidebar**'da, **navbar**'daki zilde ve **dashboard**'da gösterilir.
- Mesaj içeriği düzenlenemez (case notu gereği). Sadece okundu/okunmadı durumu değiştirilebilir. "Tümünü okundu yap" seçeneği de vardır.

### 05 · Dashboard

`Models/DashboardViewModel.cs` ile farklı tablolardaki veriler tek ekranda gösterilir:

- **İstatistik kartları:** toplam / aktif proje, yetenek, deneyim, eğitim, toplam / okunmamış mesaj (`Count`, `Where`)
- **Son 5 mesaj:** gönderen, konu, tarih, okunma durumu (`OrderByDescending`, `Take`)
- **Kategori özeti:** her kategorideki proje sayısı (`GroupBy`), Chart.js çubuk grafik + tablo
- **Son eklenen 3 proje:** ad, kategori, aktif/pasif durumu
- **Hızlı erişim:** Yeni Proje Ekle, Okunmamış Mesajları Gör, Yeni Yetenek Ekle, Ana Sayfa Bilgilerini Düzenle

### 06 · Kod kalitesi

- Create ve Update ekranları tek bir form view'ını paylaşır (ör. `ProjectForm.cshtml`). Tekrar eden kod azaltıldı.
- Görsel yükleme tek bir serviste toplandı.
- Admin ortak script dosyasındaki, sadece Deneyimler sayfasına ait olan ve diğer sayfalarda hata veren JavaScript kodu temizlendi. Eksik olan Bootstrap JS eklendi.
- Kullanılmayan dosyalar kaldırıldı (`Views/` kökündeki boş view'lar, `ProjectController1.cs`, içerik düzenleme yerine durum yönetimi tercih edildiği için `UpdateMessage`).
- Ana sayfadaki sabit (statik) HTML bölümleri View Component'lere taşındı. Kayıt yoksa sayfa hata vermez, boş durum mesajı gösterir.
- Varsayılan route `Default/Index` yapıldı. `/` adresi portfolyo sayfasını, `/Admin` adresi Dashboard'u açar.

---

## Özgün Katkılar

1. **Görsel önizleme:** Proje, Ana Sayfa ve Hakkımda formlarında görsel seçildiği anda, yüklemeden önce önizleme gösterilir (`data-preview` özelliği ile ortak script).
2. **Sayfa yenilenmeden çalışan iletişim formu:** Ziyaretçi mesaj gönderdiğinde sayfa yenilenmez. Hatalar ilgili alanın altında, başarı mesajı formun altında görünür.
3. **Dinamik yazı makinesi efekti:** Ana sayfada dönen unvanlar admin panelinden eklenip sıralanabilir ve aktif/pasif yapılabilir.
4. **Kategori silme koruması:** Projesi olan kategori silinmeye çalışıldığında, projelerin yanlışlıkla silinmesi engellenir.

---

## Kullanılan Teknolojiler

ASP.NET Core MVC 8 · Entity Framework Core 8 (Code First) · SQL Server · Bootstrap 5 · Bootstrap Icons · SweetAlert2 · Chart.js · jQuery Validation
