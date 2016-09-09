using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Cancerbero
{
  public partial class CancerberoForm : Form
  {
    public CancerberoForm()
    {
      InitializeComponent();

      IEnumerable<System.Diagnostics.Process> processes = System.Diagnostics.Process.GetProcesses();
      //  Process GENIE=  processes.Where(p => p.ProcessName.Contains("winvdm")).FirstOrDefault();

      int cancers = processes.Where(p => p.ProcessName.Contains("cancerbero")).Count();

      if (cancers > 1)
      {
        MessageBox.Show("Another Cancerbero is running. I will quit this one");
        Application.Exit();
      }
      else
      {
        System.Timers.Timer d = new System.Timers.Timer(3000);
        d.Elapsed += new System.Timers.ElapsedEventHandler(Elapsed);
        d.Enabled = true;
      }
    }

    private int crashes = 0;

    private void Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      System.Timers.Timer d = sender as System.Timers.Timer;
      d.Enabled = false;

      IEnumerable<System.Diagnostics.Process> processes = System.Diagnostics.Process.GetProcesses();
      //  Process GENIE=  processes.Where(p => p.ProcessName.Contains("winvdm")).FirstOrDefault();
      System.Diagnostics.Process k0x = processes.Where(p => p.ProcessName.Contains("k0X")).FirstOrDefault();

      bool createNew = false;

      if (k0x == null)
      {
        createNew = true;
      }
      else if (!k0x.Responding)
      {
        crashes++;
        createNew = true;
      }

      this.richTextBox1.Text = "Buggy has crashed " + crashes + " times";

      if (createNew)
      {
        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        info.WorkingDirectory = "\\\\scksrv1\\gkd\\DOKUMENT\\LRM\\_Software_Ontwikkeling\\ex_RNM_Archief\\k0X";
        info.FileName = info.WorkingDirectory + "\\" + "setup.exe";
        info.UseShellExecute = false;
        System.Diagnostics.Process pro = new System.Diagnostics.Process();
        pro.StartInfo = info;
        pro.Start();
        d.Interval = 15000;
      }
      else d.Interval = 1800000;

      d.Enabled = true;
    }
  }
}