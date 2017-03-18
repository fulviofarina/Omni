using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace k0X
{
    internal static class Program
    {
        private static bool isIdle;
        private static int counter;

        private static DateTime idleStart;

        public static bool IsIdle
        {
            get { return Program.isIdle; }
            set { Program.isIdle = value; }
        }

        public static System.Collections.Generic.IList<object> UserControls;

        public static T FindLastControl<T>(string name)
        {
            Func<T, bool> finder = o =>
            {
                bool found = false;
                UserControl os = o as UserControl;
                if (os.Name.ToUpper().CompareTo(name) == 0) found = true;
                return found;
            };
            return UserControls.OfType<T>().LastOrDefault(finder);
        }

        private static System.Windows.Forms.Timer timer;

        public static System.Windows.Forms.Timer Timer
        {
            get { return Program.timer; }
            set { Program.timer = value; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThreadAttribute]
        public static void Main()
        {
            UserControls = new System.Collections.Generic.List<object>();

            //  DB.Tools.NuDat.Query(string.Empty);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            //      Application.Idle += Application_Idle;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            killExtras();

            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 300000;
            timer.Tick += timer_Tick;

            Application.Run(new MainForm());

            // Application.SetSuspendState(PowerState.Suspend, true, false);

            //   System.ComponentModel.BackgroundWorker wo = new System.ComponentModel.BackgroundWorker();
            //   wo.DoWork += new System.ComponentModel.DoWorkEventHandler(wo_DoWork);
            //   wo.RunWorkerAsync();
        }

        private static void timer_Tick(object sender, EventArgs e)
        {
            counter++;

            int seconds = (DateTime.Now - idleStart).Seconds;

            DB.UI.LIMS.Linaa.Msg(" seconds - counter ", seconds.ToString() + " - " + counter.ToString());

            if (seconds > 2) return;

            switch (counter)
            {
                case 4:
                    {
                        //  timer.Enabled = false;
                        DB.UI.LIMS.Linaa.Speak("I am not doing anything at all.. I could calculate stuff, I just need functions here... Would you please add some?");
                        //   isIdle = false;
                    }
                    break;

                case 8:
                    {
                        //  timer.Enabled = false;
                        DB.UI.LIMS.Linaa.Speak("Aha, second stage reached. I could do a second-step calculations here, since I am bored");
                        //  isIdle = false;
                    }
                    break;

                case 12:
                    {
                        //  timer.Enabled = false;
                        DB.UI.LIMS.Linaa.Speak("Great!... The third stage was reached. I could resume stuff that were paused.... Pachamama lives!!!");
                        //  isIdle = false;
                    }
                    break;

                case 16:
                    {
                        timer.Enabled = false;
                        DB.UI.LIMS.Linaa.Speak("Have you ever been light years away?... I had to come back, but if I wanted...  I will do it again,  I will do it again, I will do it again...");
                        isIdle = false;
                    }
                    break;

                default:
                    if (counter > 16)
                    {
                        counter = 0;
                    }
                    break;
            }
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            idleStart = DateTime.Now;
            if (isIdle) return;

            isIdle = true;
            counter = 0;
            timer.Enabled = true;
            //   main.Linaa.Msg(Application.MessageLoop.ToString(), "Timer is now OFF!");
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                ShowException(e.Exception);
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private static void ShowException(Exception exc)
        {
            MessageBox.Show("An untrapped exception occurred. Please click OK to continue:\n\nMessage:\t" + exc.Message + "\nStack Trace:\t" + exc.StackTrace, "Sorry!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void killExtras()
        {
            IEnumerable<Process> processlist = Process.GetProcesses()
                      .Where(p => p.ProcessName.Contains("k0X"));
            processlist = processlist.OrderByDescending(o => o.StartTime)
                      .Skip(1);

            if (processlist.Count() >= 1)
            {
                foreach (Process theprocess in processlist)
                {
                    theprocess.Kill();
                }
            }
        }

        private static void wo_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
        }
    }
}