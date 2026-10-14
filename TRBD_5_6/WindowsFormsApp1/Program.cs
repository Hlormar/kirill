using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--verify")
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
                Environment.ExitCode = Verification.Run(args.Length > 1 ? args[1] : System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Verification"));
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
