using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualBasic;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace ThumbnailGenerator
{
    public class Program
    {
        /*
         * To update the Db folder, you can run:
         * dotnet ef dbcontext scaffold "Server=localhost;Database=impulsbroodjes;Uid=impulsbroodjes;Pwd=impulsbroodjes;" Pomelo.EntityFrameworkCore.MySql --context Db --context-dir Db --output-dir Db --namespace ThumbnailGenerator --force
         */
        public static Db db= new Db();
        public static readonly string siteBaseDir = "C:\\nginx\\webshop\\res\\thumbs\\";
        public static void Main(string[] args)
        {
            // Console.Write("Hello World!");
            // Random rnd = new Random();
            // int i = 0;
            // while (true)
            // {
            //     foreach (Broodje broodje in db.Broodjes)
            //     {
            //         db.Add(new Broodje()
            //         {
            //             BroodjeImage = "https://thearcanebrony.net/avatar.png",
            //             BroodjeIngredients = broodje.BroodjeIngredients[..3] + rnd.Next(int.MaxValue),
            //             BroodjeType = broodje.BroodjeType,
            //             BroodjePrice = (float)(rnd.NextDouble()*10),
            //             BroodjeName = broodje.BroodjeName[..3] + rnd.Next(int.MaxValue)
            //         });
            //     }
            //
            //     db.SaveChanges();
            //     Console.Write($"\r{i++} iterations, {db.Broodjes.Count()} total items");
            // }
            //Console.ReadLine();
            foreach(string file in Directory.GetFiles(siteBaseDir)) File.Delete(file);
            foreach (Broodje broodje in db.Broodjes)
            {
                //Console.WriteLine(broodje.BroodjeImage);
                string fn = ProcessImage(broodje.BroodjeId, broodje.BroodjeImage);
                broodje.BroodjeImageThumbnail128 = $"https://broodjes.thearcanebrony.net/res/thumbs/{fn}128.png";
                broodje.BroodjeImageThumbnail1024 = $"https://broodjes.thearcanebrony.net/res/thumbs/{fn}1024.png";
                if(!File.Exists($"{siteBaseDir}{fn}128.png"))
                {
                    File.Copy($"{fn}128.png", $"{siteBaseDir}{fn}128.png");
                    File.Copy($"{fn}1024.png", $"{siteBaseDir}{fn}1024.png");   
                }
            }
            db.SaveChanges();
        }

        private static WebClient wc = new WebClient();
        private static Dictionary<String, String> alreadyFinishedUrls = new();
        public static string ProcessImage(int id, string url)
        {
            string filename = id +"."+ url.Split('.').Last().Split("?").First();
            if (alreadyFinishedUrls.ContainsKey(url))
            {
                foreach (int size in new int[]{128, 1024})
                {
                    //File.Copy(alreadyFinishedUrls[url]+size+".png", filename.Split(".").First() + $"_{size}.png", true);
                    File.Delete(filename.Split(".").First() + $"_{size}.png");
                    CreateSymbolicLink(filename.Split(".").First() + $"_{size}.png", alreadyFinishedUrls[url] + size + ".png", 0x0);
                }
                return alreadyFinishedUrls[url];
            }
            if(!File.Exists(filename)) wc.DownloadFile(url, filename);
            Console.WriteLine("Downloaded " + filename);
            foreach (int size in new int[]{128, 1024})
            {
                ResizeImage(filename, (int)size);
            }
            alreadyFinishedUrls.Add(url, filename.Split(".").First()+"_");
            return alreadyFinishedUrls[url];
        }

        static void ResizeImage(string filename, int size)
        {
            Image image = Image.FromFile(filename);
            Bitmap bitmap = new Bitmap(size, size);
            int sourceSize = Math.Min(image.Width, image.Height);
            int x = image.Width > image.Height ? (image.Width - image.Height) / 2 : 0;
            int y = image.Height > image.Width ? (image.Height - image.Width) / 2 : 0;
            
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.Clamp);
                    graphics.DrawImage(image, new Rectangle(0,0,size,size), x, y, sourceSize,sourceSize, GraphicsUnit.Pixel, wrapMode);
                }
            }
            bitmap.Save(filename.Split(".").First() + $"_{size}.png", ImageFormat.Png);
            Console.WriteLine($"Resized {filename}!");
        }
        
        //create symlink
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I1)]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, UInt32 dwFlags);
    }
}