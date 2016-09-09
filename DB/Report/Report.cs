using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Msn;

namespace DB
{
  public partial class LINAA
  {
    /// <summary>
    //SHITTY
    ///SHITTY
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private static string[] findGreeting(ref string user)
    {
      string text;
      string title;
      int hours = DateTime.Now.TimeOfDay.Hours;
      if (hours < 12) text = "Good morning " + user;
      else if ((hours < 16) && (hours >= 12)) text = "Good afternoon... " + user;
      else text = "Good evening... " + user;

      text += "\nPlease type in the name of the project";
      title = "Which project to look for?";

      return new string[] { title, text };
    }

    //SHITTY
    /// <summary>
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <param name="pref"></param>
    /// <returns></returns>
    private string findHellos(ref string user, ref LINAA.PreferencesRow pref)
    {
      IEnumerable<LINAA.HelloWorldRow> hellos = pref.GetHelloWorldRows();

      int count = hellos.Count();
      if (this.HelloWorld.Rows.Count != count)
      {
        IEnumerable<LINAA.HelloWorldRow> toremove = this.HelloWorld.Except(hellos);
        this.Delete(ref toremove);
      }

      string comment = String.Empty;

      if (count != 0 && count < 20)
      {
        Random random = new Random();
        int index = random.Next(0, count);
        comment = hellos.ElementAt(index).Comment;
        // user = hellos.ElementAt(index).k0User;
      }

      return comment;
    }
  }

  public partial class LINAA : IReport
  {
    public void ReportFinished()
    {
      if (Exceptions.Count != 0)
      {
        Speak("Loading finished! However... some errors were found");
      }
      else Speak("Loading finished!");

      try
      {
        string user = this.currentPref.WindowsUser;
        LINAA.PreferencesRow p = this.currentPref;

        string comment = findHellos(ref user, ref p);

        string[] txtTitle = findGreeting(ref user);

        Msg(txtTitle[0], txtTitle[1]);
      }
      catch (SystemException ex)
      {
        AddException(ex);
      }
    }

    public bool IsSpectraPathOk
    {
      get
      {
        string spec = currentPref.Spectra;
        if (string.IsNullOrEmpty(spec)) return false;
        else return Directory.Exists(spec);
      }
    }

    //  private System.Drawing.Icon mainIcon = null;

    private NotifyIcon notify = null;

    public NotifyIcon Notify
    {
      get { return notify; }
      set
      {
        //   if (value == null) return;
        notify = value;

        //      if (notify.Icon == null) return;
        //  if (mainIcon == null) mainIcon = notify.Icon;
      }
    }

    private Pop msn = null;

    public Pop Msn
    {
      get { return msn; }
      set { msn = value; }
    }

    public void Msg(string msg, string title, bool ok)
    {
      this.msn.Msg(msg, title, ok);
    }

    /// <summary>
    /// Notifies the given message and title with an Info icon
    /// </summary>
    /// <param name="msg">Message to display</param>
    /// <param name="title">title of the message</param>
    public void Msg(string msg, string title)
    {
      this.msn.Msg(msg, title, true);
    }

    public void Speak(string text)
    {
      this.msn.Speak(text);
    }

    public void ReportProgress(int percentage)
    {
      this.msn.ReportProgress(percentage);
    }
  }
}