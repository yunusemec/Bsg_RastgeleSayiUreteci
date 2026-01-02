# Bsg_RastgeleSayiUreteci

# XorShift64* Pseudo-Random Sayı Üreteci (C#)

Bu proje, **XorShift64*** algoritmasını kullanarak pseudo-random sayı üretir.  
XorShift, bit düzeyinde kaydırma (shift) ve XOR işlemleriyle state'i günceller. `*` (star) versiyonunda,
çıktı son adımda sabit bir sayı ile çarpılarak dağılımın (mixing) iyileştirilmesi hedeflenir.

## Algoritma Özeti
State (64-bit) her çağrıda şu şekilde güncellenir:

- `x ^= x >> 12`
- `x ^= x << 25`
- `x ^= x >> 27`
- çıktı: `x * 2685821657736338717`

## Özellikler
- `NextUInt64()` : 64-bit ham çıktı
- `NextDouble()` : [0,1) aralığında double
- `NextInt(min,max)` : [min, max) aralığında int

Ayrıca program sonunda küçük bir **mini test** ile 0..99 aralığında dağılım/histogram yazdırılır.

## Kaynakça
Bu çalışmada, pseudo-random sayı üreteçleri ve kriptolojiye giriş konularının teorik altyapısını oluşturmak amacıyla Prof. Dr. Fatih Özkaynak tarafından hazırlanan Kriptoloji Bilimine Giriş adlı video ders serisi incelenmiştir.

Erişim adresi:
https://www.youtube.com/watch?v=0RECW49LmHM&list=PLR_3k5Bkz0SAgl6aeXR-4_3Gtv9rywoBa

## Çalıştırma
```bash
dotnet run

