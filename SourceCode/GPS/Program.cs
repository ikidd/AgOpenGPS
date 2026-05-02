using AgOpenGPS.Forms;
using AgLibrary.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly Mutex Mutex = new Mutex(true, "{516-0AC5-B9A1-55fd-A8CE-72F04E6BDE8F}");

        public static readonly string Version = Assembly.GetEntryAssembly().GetName().Version.ToString(3); // Major.Minor.Patch
        public static readonly string SemVer = Application.ProductVersion.Split('+').First();
        public static readonly bool IsPreRelease = Application.ProductVersion.Contains('-');
        public static readonly bool IsDevelopVersion = Application.ProductVersion == "1.0.0.0";

        [STAThread]
        private static void Main()
        {
            try
            {
                if (Mutex.WaitOne(TimeSpan.Zero, true))
                {
                    RegistrySettings.Load();
                    InstallGlobalExceptionHandlers();

                    Log.EventWriter("Main startup: registry loaded, global exception handlers installed");
                    Log.FileSaveSystemEvents();

                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    Log.EventWriter("DIAGNOSTIC: Application.Run starting");
                    Log.FileSaveSystemEvents();
                    Application.Run(new FormGPS());
                    Log.EventWriter("DIAGNOSTIC: Application.Run returned");
                    Log.FileSaveSystemEvents();
                }
                else
                {
                    FormDialog.Show(
                        "Warning",
                        "AgOpenGPS is Already Running",
                        DialogSeverity.Warning);
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("FATAL startup exception: " + ex);
                Log.FileSaveSystemEvents();
                MessageBox.Show(ex.ToString(), "AgOpenGPS fatal startup exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void InstallGlobalExceptionHandlers()
        {
            Application.ThreadException += (sender, e) =>
            {
                Log.EventWriter("UNHANDLED UI THREAD EXCEPTION: " + e.Exception);
                Log.FileSaveSystemEvents();
                MessageBox.Show(e.Exception.ToString(), "AgOpenGPS unhandled UI exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            Application.ApplicationExit += (sender, e) =>
            {
                Log.EventWriter("DIAGNOSTIC: ApplicationExit event fired");
                Log.FileSaveSystemEvents();
            };

            Application.ThreadExit += (sender, e) =>
            {
                Log.EventWriter("DIAGNOSTIC: UI ThreadExit event fired");
                Log.FileSaveSystemEvents();
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Log.EventWriter("DIAGNOSTIC: ProcessExit event fired");
                Log.FileSaveSystemEvents();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.EventWriter("UNHANDLED APPDOMAIN EXCEPTION: " + e.ExceptionObject);
                Log.FileSaveSystemEvents();
            };
        }
    }
}