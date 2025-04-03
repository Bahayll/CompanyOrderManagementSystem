# Company Order Management Service

- Company Order Management Service, şirketlerin ürün ve sipariş süreçlerini verimli bir şekilde yönetmelerine olanak tanıyan bir backend uygulamasıdır. Bu proje, şirketlerin belirlenen zaman aralıklarında sipariş alabilmesini, stok takibini yapabilmesini ve sipariş süreçlerini optimize edebilmesini sağlamak amacıyla geliştirilmiştir.
- ASP.NET Core 6, Entity Framework Core, Mediatr ve FluentValidation gibi modern teknolojilerle geliştirilen bu sistem, CQRS mimarisini kullanarak performanslı ve ölçeklenebilir bir altyapı sunmaktadır. Uygulama, Repository Design Pattern ve Unit of Work prensiplerini benimseyerek temiz ve yönetilebilir bir kod tabanına sahiptir.

## Proje Özellikleri

- Bu proje, şirketlerin sipariş ve ürün yönetimini kolaylaştırmak için geliştirilmiştir ve aşağıdaki temel özellikleri içermektedir:

### Şirket Yönetimi:

- Yeni şirket ekleme

- Şirket bilgilerini güncelleme (onay durumu, sipariş izin saatleri vb.)

- Şirketleri listeleme

- Şirket silme (bağlı sipariş ve ürünleriyle birlikte)

### Ürün Yönetimi:

- Şirketlere özel ürün ekleme

- Stok ve fiyat güncelleme

- Ürünleri listeleme

-Ürün silme (ilgili siparişler de silinir)

### Sipariş Yönetimi:

- Sipariş oluşturma

- Siparişlerin durumunu takip etme

- Siparişleri listeleme ve filtreleme

- Sipariş silme

### Sipariş kısıtlamaları:

- Şirketin onay durumu kontrol edilir. Onaylanmamış şirketlere sipariş verilemez.

- Şirketin sipariş kabul saatleri kontrol edilir. Belirtilen saatler dışında sipariş alınamaz.

- Şirketin sattığı ürünler haricinde sipariş oluşturulamaz.

## Gelişmiş Mimari ve Teknoloji Kullanımı:

- CQRS ve Mediatr kullanımıyla performanslı veri işleme

- Onion Architecture prensipleriyle modüler ve ölçeklenebilir yapı

- FluentValidation ile güvenilir veri doğrulama

- AutoMapper kullanarak verimli veri dönüşümleri

## Kullanılan Teknolojiler

- Backend: .NET Core, ASP.NET Web API

- Veritabanı: MSSQL ve Entity Framework Core kullanılarak geliştirilmiştir. Farklı veritabanlarıyla entegrasyona uygun esnek bir yapı sunar.

- Mimari: Onion Architecture, Repository Pattern, Unit of Work, CQRS, Mediatr, FluentValidation, AutoMapper ve  JWT Authentication.

- Test: xUnit, Moq

## Kurulum ve Çalıştırma

Projeyi yerel ortamda çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

- Projeyi klonlayın:

   git clone https://github.com/kullaniciadi/CompanyOrderManagementService.git
   cd CompanyOrderManagementService

- Bağımlılıkları yükleyin:

   dotnet restore

- Veritabanını oluşturun:

   dotnet ef database update

- Projeyi çalıştırın:

   dotnet run

Swagger UI üzerinden API’yi test edibilirsiniz.
