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

## Çalıştırma
```bash
dotnet run

