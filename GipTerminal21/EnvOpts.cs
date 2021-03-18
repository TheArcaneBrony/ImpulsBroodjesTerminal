using System.Diagnostics;
using System.IO;
using System.Security.Principal;

namespace GipTerminal21
{
    public class EnvOpts
    {
        public static bool IsHost => File.Exists("C:\\.arcane");
        public static bool IsRelease = false;
    }
}