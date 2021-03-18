﻿using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace GipTerminalInstaller
{
    public sealed class Wallpaper
    {
        Wallpaper() { }

        // ReSharper disable InconsistentNaming
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;
        // ReSharper restore InconsistentNaming

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style
        {
            Tiled,
            Centered,
            Stretched
        }
        static Random rnd = new Random();
        public static void Set(string uri, Style style)
        {
            Stream s = new WebClient().OpenRead(uri);

            Image img = Image.FromStream(s);
            string tempPath = Path.Combine(Path.GetTempPath(), $"wallpaper{rnd.Next(int.MaxValue)}.bmp");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            Util.Log($"Set wallpaper to {uri}");
        }
    }

}