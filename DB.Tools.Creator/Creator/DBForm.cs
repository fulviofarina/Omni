using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTools;
using System.Windows.Forms;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

using System.Data;

using System.IO;

using DB.Properties;
using DB.UI;
using Rsx.Dumb;


using VTools;
namespace DB.Tools
{
   public  class DBForm : System.Windows.Forms.Form
    {
        public static string TITLE = System.Windows.Forms.Application.ProductName + " v" + System.Windows.Forms.Application.ProductVersion + " (Beta)";
        public static Form CreateForm(ref Bitmap image)
        {
            Form form = new DBForm(ref image, TITLE);

            /*
            form.Opacity = 0;
            form.AutoSizeMode = AutoSizeMode.GrowOnly;
            form.AutoSize = true;
            IntPtr Hicon = image.GetHicon();
            Icon myIcon = Icon.FromHandle(Hicon);
            form.Icon = myIcon;
            form.Text = TITLE;
            form.HelpButton = true;
            form.TopMost = false;
            form.ShowInTaskbar = true;
            form.ShowIcon = true;
            form.MaximizeBox = false;
            form.ControlBox = true;
            form.StartPosition = FormStartPosition.CenterParent;
            form.SetDesktopLocation(0, 0);
            */
            return form;
        }
        public DBForm(ref Bitmap image, string TITLE) : base()
        {
            Opacity = 0;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            AutoSize = true;
            IntPtr Hicon = image.GetHicon();
            Icon myIcon = Icon.FromHandle(Hicon);
            Icon = myIcon;
            Text = TITLE;
            HelpButton = true;
            TopMost = false;
            ShowInTaskbar = true;
            ShowIcon = true;
            MaximizeBox = false;
            ControlBox = true;
            StartPosition = FormStartPosition.CenterParent;
            SetDesktopLocation(0, 0);
        }

    }

}
