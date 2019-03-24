using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Console;

namespace Yilan
{
    class Program
    {
        public static string tempdosyasi = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Kayit.ini");
        static void Main()
        {
            WindowHeight = 16;
            WindowWidth = 32;
            var rasgele = new Random();
            var skor = 0;
            int eniyiskor = 0;
            var hiz = 400;
            Title = Convert.ToString("Müthiş Yılan Oyunu");
            var bas = new Piksel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            var kenarlar = new Piksel(rasgele.Next(1, WindowWidth - 10), rasgele.Next(1, WindowHeight - 10), ConsoleColor.DarkGray);
            var ortayer = new List<Piksel>();
            var ilkHareket = Yon.Sag;
            var oyunBittiMi = false;
            if (File.Exists(tempdosyasi))
            {
                TextReader tr = new StreamReader(tempdosyasi);
                string a1 = tr.ReadLine();
                eniyiskor = Convert.ToInt32(a1);
                tr.Close();
            }
            else
            {
                TextWriter tw = new StreamWriter(tempdosyasi);
                tw.Close();
            }
            if (eniyiskor <= 25)
                hiz = 350;
            else if (eniyiskor <= 50)
                hiz = 300;
            else if (eniyiskor <= 75)
                hiz = 250;
            else if (eniyiskor <= 100)
                hiz = 200;
            else if (eniyiskor <= 200)
                hiz = 100;
            SetCursorPosition(WindowWidth / 5 - 3, WindowHeight / 2 - 2);
            Write("Kendini hazır hissediyorsan");
            SetCursorPosition(WindowWidth / 5 + 3, WindowHeight / 2 + 1);
            WriteLine("Bir tuşa bas");
            ReadKey();
            while (true)
            {
                Clear();
                oyunBittiMi |= (bas.XPoz == WindowWidth - 1 || bas.XPoz == 0 || bas.YPoz == WindowHeight - 1 || bas.YPoz == 0);
                DuvarCiz();
                if (kenarlar.XPoz == bas.XPoz && kenarlar.YPoz == bas.YPoz)
                {
                    skor++;
                    Title = Convert.ToString($"Skor: {skor}");
                    kenarlar = new Piksel(rasgele.Next(1, WindowWidth - 10), rasgele.Next(1, WindowHeight - 10), ConsoleColor.DarkGray);
                }
                for (int i = 0; i < ortayer.Count; i++)
                {
                    PikselCiz(ortayer[i]);
                    oyunBittiMi |= (ortayer[i].XPoz == bas.XPoz && ortayer[i].YPoz == bas.YPoz);
                }
                if (oyunBittiMi)
                {
                    break;
                }
                PikselCiz(bas);
                PikselCiz(kenarlar);
                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds <= hiz)
                {
                    ilkHareket = HareketOku(ilkHareket);
                }
                ortayer.Add(new Piksel(bas.XPoz, bas.YPoz, ConsoleColor.Green));
                switch (ilkHareket)
                {
                    case Yon.Yukari:
                        bas.YPoz--;
                        break;
                    case Yon.Asagi:
                        bas.YPoz++;
                        break;
                    case Yon.Sol:
                        bas.XPoz--;
                        break;
                    case Yon.Sag:
                        bas.XPoz++;
                        break;
                }
                if (ortayer.Count > skor)
                {
                    ortayer.RemoveAt(0);
                }
            }
            if (skor > eniyiskor)
            {
                eniyiskor = skor;
                TextWriter tw = new StreamWriter(tempdosyasi);
                tw.WriteLine(eniyiskor);
                tw.Close();
            }
            Clear();
            DuvarCiz();
            SetCursorPosition(WindowWidth / 5 + 1, WindowHeight / 2 - 2);
            WriteLine($"Oyun bitti, skor: {skor}");
            SetCursorPosition(WindowWidth / 5 + 2, WindowHeight / 2);
            WriteLine($"En yüksek skor: {eniyiskor}");
            ReadKey();
        }
        static Yon HareketOku(Yon hareket)
        {
            if (KeyAvailable)
            {
                var key = ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && hareket != Yon.Asagi)
                {
                    hareket = Yon.Yukari;
                }
                else if (key == ConsoleKey.DownArrow && hareket != Yon.Yukari)
                {
                    hareket = Yon.Asagi;
                }
                else if (key == ConsoleKey.LeftArrow && hareket != Yon.Sag)
                {
                    hareket = Yon.Sol;
                }
                else if (key == ConsoleKey.RightArrow && hareket != Yon.Sol)
                {
                    hareket = Yon.Sag;
                }
            }
            return hareket;
        }
        static void PikselCiz(Piksel Piksel)
        {
            SetCursorPosition(Piksel.XPoz, Piksel.YPoz);
            ForegroundColor = Piksel.EkranRengi;
            Write("@");
            SetCursorPosition(0, 0);
        }
        static void DuvarCiz()
        {
            for (int i = 0; i < WindowWidth; i++)
            {
                SetCursorPosition(i, 0);
                Write("■");
                SetCursorPosition(i, WindowHeight - 1);
                Write("■");
            }

            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(0, i);
                Write("■");
                SetCursorPosition(WindowWidth - 1, i);
                Write("■");
            }
        }
        struct Piksel
        {
            public Piksel(int xPoz, int yPoz, ConsoleColor color)
            {
                XPoz = xPoz;
                YPoz = yPoz;
                EkranRengi = color;
            }
            public int XPoz { get; set; }
            public int YPoz { get; set; }
            public ConsoleColor EkranRengi { get; set; }
        }
        enum Yon
        {
            Yukari,
            Asagi,
            Sag,
            Sol
        }
    }
}