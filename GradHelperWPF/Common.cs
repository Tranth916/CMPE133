using System.IO;

namespace GradHelperWPF
{
    public static class Common
    {
        public static readonly string SessionFileName = "Session.XML";

        public static bool LoadSessionFile( )
        {
            // look for the current session file in the working directory.
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), SessionFileName,
                SearchOption.AllDirectories);

            // if not found then lets create it.

            return false;
        }
    }
}