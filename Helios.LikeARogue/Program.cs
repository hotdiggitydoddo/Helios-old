using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Helios.LikeARogue
{
    class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_OnUnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += AppDomain_OnAssemblyResolve;
        }

        static void Main(string[] args)
        {
            try
            {
                RunSafe(args);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private static void RunSafe(string[] args)
        {
            var game = new LikeARogue();
            game.Run();
        }

        private static void AppDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        private static Assembly AppDomain_OnAssemblyResolve(object sender, ResolveEventArgs e)
        {
            var asmName = Path.Combine(
                Environment.Is64BitProcess ? "x64" : "x32",
                new AssemblyName(e.Name).Name + ".dll");
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var asmPath = Path.Combine(folder, asmName);
            if (!File.Exists(asmPath))
            {
                return null;
            }
            return Assembly.LoadFrom(asmPath);
        }
    }
}
