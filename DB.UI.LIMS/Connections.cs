using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DB.UI
{
  public partial class Connections : Form
  {
    private DB.LINAA.PreferencesRow pref;
  //  private bool saveChanges = false;

    public Connections(ref object preferences)
    {
      InitializeComponent();
      pref = (DB.LINAA.PreferencesRow)preferences;
    }

    private void Connections_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.LimsRTB.Text.Equals(string.Empty))
      {
        DialogResult res = MessageBox.Show("The LIMS database needs to be provided... Try again?", "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        if (res == System.Windows.Forms.DialogResult.No) this.Dispose();
        else return;
      }

      try
      {
        pref.HL = HyperLabRTB.Text;
        pref.LIMS = this.LimsRTB.Text;

        pref.SpectraSvr = this.SpectraSvrRBT.Text.ToUpper();

        string spectra = this.SpectraRBT.Text.ToUpper();
        if (!spectra[spectra.Length - 1].ToString().Equals("\\")) spectra += "\\";
        pref.Spectra = spectra;
      }
      catch (SystemException ex)
      {
      }
      this.Dispose();
    }

    private void Connections_Load(object sender, EventArgs e)
    {
      try
      {
        HyperLabRTB.Text = pref.HL;
        LimsRTB.Text = pref.LIMS;
        SpectraSvrRBT.Text = pref.SpectraSvr;
        SpectraRBT.Text = pref.Spectra;

        string[] hstring = HyperLabRTB.Text.Split(';');
        string[] lstring = LimsRTB.Text.Split(';');

        string[] arr = hstring;

        hsrv.Text = arr[0].Split('=')[1];
        hdb.Text = arr[1].Split('=')[1];
        hlogin.Text = arr[3].Split('=')[1];
        hpass.Text = arr[4].Split('=')[1];

        IEnumerable<TextBox> bxs = null;

        bxs = htlp.Controls.OfType<TextBox>().ToList();

        foreach (TextBox t in bxs) t.Tag = t.Text;

        arr = lstring;
        lsrv.Text = arr[0].Split('=')[1];
        ldb.Text = arr[1].Split('=')[1];
        llogin.Text = arr[3].Split('=')[1];
        lpass.Text = arr[4].Split('=')[1];

        bxs = ltlp.Controls.OfType<TextBox>().ToList();

        foreach (TextBox t in bxs) t.Tag = t.Text;
      }
      catch (SystemException ex)
      {
      }
    }

    private void hsrv_TextChanged(object sender, EventArgs e)
    {
      string tag = string.Empty;
      string text = string.Empty;

      TextBox box = sender as TextBox;
      if (box.Tag == null) return;
      if (box.Text.Equals(string.Empty)) return;
      if (box.Text.Length < 3) return;

      tag = box.Tag as string;
      text = box.Text;
      box.Tag = text;

      Control control = sender as Control;
      if (control.Name[0] == ('h'))
      {
        HyperLabRTB.Text = HyperLabRTB.Text.Replace("=" + tag, "=" + text);
      }
      else this.LimsRTB.Text = this.LimsRTB.Text.Replace("=" + tag, "=" + text);
    }
  }
}