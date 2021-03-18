using System;
using System.Diagnostics;
using System.IO;
using ThumbnailGenerator;

namespace UserGenerator
{
    class Program
    {
        private static string[] Users = new []{"invalid_user"};
        static void Main(string[] args)
        {
            Console.WriteLine(ScrambleString("Hello World!", 16));
            if (File.Exists("users.txt"))
            {
                Console.WriteLine("Reading user list...");
                PreInit();
                GenerateUsers();
            }
            else
            {
                Console.WriteLine("User list (users.txt) does not exist!");
            }
        }

        static void PreInit()
        {
            Users = File.ReadAllLines("users.txt");
            if(File.Exists("out.txt")) File.Delete("out.txt");
            outputFile = new StreamWriter("out.txt");
        }

        private static StreamWriter outputFile;
        private static Db db = new Db();
        static void GenerateUsers()
        {
            foreach (string user in Users)
            {
                string[] nameParts = user.Split(" ", 2);
                string firstname = nameParts[0];
                string lastname = nameParts[1];
                string email = $"{lastname.Replace(" ", "")}.{firstname}@campusimpuls.be".ToLower();
                string passwordTemplate = $"{firstname}";
                for (int i = 0; i < 4; i++)
                {
                    passwordTemplate += rnd.Next(10);
                }
                (string plain, string hashed) password = ScrambleString(passwordTemplate, 1006);
                db.Users.Add(new User()
                {
                    UserEmail = email,
                    UserFirstname = firstname,
                    UserLastname = lastname,
                    UserPassword = password.hashed
                });
                outputFile.WriteLine($"Login: {email}\n" +
                                     $"Wachtwoord: {password.plain}");
                Console.WriteLine($"{email}:{password.plain}");
                db.SaveChanges();
            }
            outputFile.Flush();
            outputFile.Close();
        }

        static Random rnd = new();
        static (string plain, string hashed) ScrambleString(string input, int strength)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            char[] inp = input.ToCharArray();
            for (int i = 0; i < input.Length * strength; i++)
            {
                int ia = rnd.Next(input.Length), ib = rnd.Next(input.Length);
                char sa = inp[ia], sb = inp[ib];
                inp[ia] = sb;
                inp[ib] = sa;
            }

            string outp = new string(inp);
            Debug.WriteLine($"Scrambled string[{input.Length}] str {strength} in {sw.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond} ms with output {outp}.");
            sw.Restart();
            string hashed = BCrypt.Net.BCrypt.HashPassword(outp, 10);
            Debug.WriteLine($"Hashed password {sw.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond} ms.");
            return (outp, hashed);
        }
    }
}