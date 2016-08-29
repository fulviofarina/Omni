using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Rsx
{
    public class Tim : System.Windows.Forms.Timer
    {
        public Tim(int seg, Action toDo)
        {
            this.Tick += new EventHandler(Tim_Tick);
            this.Interval = seg * 1000;
        }

        private void Tim_Tick(object sender, EventArgs e)
        {
            if (afterTick == null) return;
            afterTick.Invoke();
            afterTick = null;
            this.Dispose();
        }

        private Action afterTick;

        public Action AfterTick
        {
            get { return afterTick; }
            set { afterTick = value; }
        }
    }

    /// <summary>
    /// A Worker made for multiple methods
    /// </summary>
    ///

}