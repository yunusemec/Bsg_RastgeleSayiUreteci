using System;
using System.Diagnostics;

namespace XorShiftRNG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== XorShift64* Pseudo-Random Sayi Ureteci ===");

            // Seed: kullanıcı girerse onu kullan, girmezse otomatik
            Console.Write("Seed gir (bos birak = otomatik): ");
            string? input = Console.ReadLine();

            ulong seed;
            if (!string.IsNullOrWhiteSpace(input) && ulong.TryParse(input, out ulong userSeed) && userSeed != 0)
                seed = userSeed;
            else
                seed = SeedFromEnvironment();

            var rng = new XorShift64Star(seed);

            Console.Write("Kac adet sayi uretilsin? (or: 10): ");
            string? countStr = Console.ReadLine();

            int count = 10;
            if (!string.IsNullOrWhiteSpace(countStr) && int.TryParse(countStr, out int c) && c > 0)
                count = c;

            Console.WriteLine($"\nSeed: {seed}");
            Console.WriteLine($"Uretilen {count} sayi (0..int.MaxValue):\n");

            for (int i = 0; i < count; i++)
            {
                // Örnek: 0..1_000_000 arası üretelim (istersen değiştir)
                int n = rng.NextInt(0, 1_000_000);
                Console.Write(n);
                if (i < count - 1) Console.Write(", ");
            }

            Console.WriteLine("\n\n--- Mini Test ---");
            MiniTest(rng);

            Console.WriteLine("\nBitti.");
        }

        // XorShift64* PRNG
        // Not: Kriptografik amaçlı değildir (ödev/simülasyon amaçlı).
        public sealed class XorShift64Star
        {
            private ulong _state;

            public XorShift64Star(ulong seed)
            {
                // state 0 olursa xorshift kilitlenebilir, engelle
                _state = seed == 0 ? 0x9E3779B97F4A7C15UL : seed;
            }

            // 64-bit ham çıktı
            public ulong NextUInt64()
            {
                ulong x = _state;

                // xorshift64
                x ^= x >> 12;
                x ^= x << 25;
                x ^= x >> 27;

                _state = x;

                // yıldız (*) karıştırma: sabit çarpan ile dağılımı güçlendirir
                return x * 2685821657736338717UL;
            }

            // 0.0 <= double < 1.0
            public double NextDouble()
            {
                // üst 53 bit -> double mantissa
                ulong r = NextUInt64();
                return (r >> 11) * (1.0 / (1UL << 53));
            }

            // [min, max) aralığında int üretir
            public int NextInt(int minInclusive, int maxExclusive)
            {
                if (minInclusive >= maxExclusive)
                    throw new ArgumentException("minInclusive < maxExclusive olmali.");

                ulong range = (ulong)(maxExclusive - minInclusive);
                ulong value = NextUInt64();

                // Mod ile aralığa indirgeme (ödev için yeterli)
                int result = (int)(value % range) + minInclusive;
                return result;
            }
        }

        // Daha sağlam otomatik seed üretimi
        private static ulong SeedFromEnvironment()
        {
            ulong t = (ulong)DateTime.UtcNow.Ticks;
            ulong pid = (ulong)Environment.ProcessId;
            ulong mem = (ulong)GC.GetTotalMemory(false);
            ulong tick = (ulong)Environment.TickCount64;

            // Biraz karıştır
            ulong seed = t ^ (pid << 16) ^ (mem << 32) ^ tick;
            if (seed == 0) seed = 0xD1B54A32D192ED03UL;
            return seed;
        }

        // Çok kısa bir dağılım kontrolü: 10.000 sayı üret, ortalama vb.
        private static void MiniTest(XorShift64Star rng)
        {
            const int N = 10000;
            long sum = 0;
            int min = int.MaxValue;
            int max = int.MinValue;

            // 0..100 arası histogram
            int[] buckets = new int[10]; // 0-9,10-19,...90-99 (100 dahil değil)

            for (int i = 0; i < N; i++)
            {
                int x = rng.NextInt(0, 100);
                sum += x;
                if (x < min) min = x;
                if (x > max) max = x;

                int b = x / 10;
                if (b == 10) b = 9;
                buckets[b]++;
            }

            double avg = sum / (double)N;

            Console.WriteLine($"N={N} (0..99 arasi test)");
            Console.WriteLine($"Min: {min}, Max: {max}, Ortalama: {avg:F2}");
            Console.WriteLine("Histogram (10'luk dilimler):");

            for (int i = 0; i < buckets.Length; i++)
            {
                int start = i * 10;
                int end = start + 9;
                Console.WriteLine($"{start:D2}-{end:D2}: {buckets[i]}");
            }
        }
    }
}
