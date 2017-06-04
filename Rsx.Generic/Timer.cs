using System;
using System.Timers;


namespace Rsx.Generic
{
  public class Timer : System.Timers.Timer
  {


     

        public Timer(int seg, Action toDo)
    {
            this.Elapsed += Tim_Tick;
      this.Interval = seg * 1000;
    }

        private void Tim_Tick(object sender, ElapsedEventArgs e)
        {
            if (afterTick == null) return;
            afterTick.Invoke();
            afterTick = null;
            this.Dispose();
        }


        //  private void Tim_Tick(object sender, EventArgs e)

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