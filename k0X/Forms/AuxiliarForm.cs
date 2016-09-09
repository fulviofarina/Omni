using System;
using System.Drawing;
using System.Windows.Forms;
using DB.UI;

namespace k0X
{
  public partial class AuxiliarForm : Form
  {
    public UserControl DisplayedControl = null;

    public AuxiliarForm()
    {
      InitializeComponent();
    }

    private int PreviousMovements = 1;
    private int NewMovements = 0;

    private Point PreviousLocation = Point.Empty;
    private Size OriginalSize = Size.Empty;
    private Size HiddenSize = Size.Empty;

    public void Populate(ref UserControl control)
    {
      this.AutoSizeMode = AutoSizeMode.GrowOnly;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      TLP.Controls.Clear();
      control.Dock = DockStyle.Fill;
      TLP.Controls.Add(control, 0, 0);
      DisplayedControl = control;
      this.AutoSizeMode = AutoSizeMode.GrowOnly;
      OriginalSize = new Size(control.Size.Width, control.Size.Height);
      HiddenSize = new Size(control.Size.Width, 0);
    }

    public void AuxiliarForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      Type typo = this.DisplayedControl.GetType();

      if (typo.Equals(typeof(IPeaks)))
      {
        IPeaks project = (IPeaks)this.DisplayedControl;
        try
        {
          //   Rsx.Dumb.HasChanges(project.Linaa;
          //  if (changes.Length != 0)
          {
            DialogResult result = MessageBox.Show("Changes for has not been saved yet\n\nDo you want to save the changes?", "Changes detected...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) project.Save();
          }
        }
        catch (SystemException ex)
        {
          MessageBox.Show(ex.Message);
        }
        this.Visible = false;

        e.Cancel = true;
      }
      else if (typo.Equals(typeof(ucOrder)))
      {
        ucOrder order = (ucOrder)this.DisplayedControl;
        foreach (System.Collections.DictionaryEntry ucSamDict in order.ucSamplesHashTable)
        {
          try
          {
            ucSamples ucSam = (ucSamples)ucSamDict.Value;
            ucSam.ParentForm.Dispose();
          }
          catch (SystemException ex)
          {
            string ef = ex.Message;
          }
        }
        order.Save();
        e.Cancel = false;
      }
      else if (typo.Equals(typeof(ucToDoData)))
      {
        ucToDoPanel TodoPanel = (ucToDoPanel)((ucToDoData)this.DisplayedControl).Daddy; //TAG IS DAD
        TodoPanel.View_Click(sender, EventArgs.Empty);
        e.Cancel = true;
      }
      else if (typo.Equals(typeof(ucToDoPanel)))
      {
        ucToDoPanel TodoPanel = ((ucToDoPanel)this.DisplayedControl);
        TodoPanel.ucToDoData.Dispose();
        TodoPanel.ucToDoData = null;
        e.Cancel = false;
      }
      else if (typo.Equals(typeof(ucSubSamples)))
      {
        if (this.DisplayedControl != null)
        {
          DB.UI.ISubSamples iS = (DB.UI.ISubSamples)this.DisplayedControl;
          ucSamples samples = iS.Daddy as ucSamples;
          samples.ViewLarge_Click(sender, e);
          e.Cancel = true;
        }
        else e.Cancel = false;
      }
      else if (typo.Equals(typeof(ucSamples)))
      {
        ucSamples samples = this.DisplayedControl as ucSamples;
        e.Cancel = false;
      }
      if (!e.Cancel)
      {
        this.DisplayedControl.Dispose();
        this.DisplayedControl = null;
      }
    }

    private void AuxiliarForm_LocationChanged(object sender, EventArgs e)
    {
      NewMovements++;
    }

    public void AuxiliarForm_MouseCaptureChanged(object sender, EventArgs e)
    {
      Type tipo = this.DisplayedControl.GetType();
      if (!tipo.Equals(typeof(ucSamples))) return;

      ucSamples samples = ((ucSamples)DisplayedControl);

      if (NewMovements != PreviousMovements)   //is moving...
      {
        if (samples.Visible)
        {
          samples.Visible = false;
          this.AutoSizeMode = AutoSizeMode.GrowOnly;
          this.Size = this.HiddenSize;
        }
        else if (NewMovements != 0)
        {
          PreviousMovements = NewMovements;
        }
        if (NewMovements == PreviousMovements)
        {
          NewMovements = 0;
          PreviousMovements = 0;
          if (samples.ProjectsRow != null)
          {
            samples.ProjectsRow.X = this.Location.X;
            samples.ProjectsRow.Y = this.Location.Y;
          }
          if (!samples.Visible)
          {
            samples.Visible = true;
            this.Size = this.OriginalSize;
            PreviousMovements++;
          }
        }
      }
      else
      {
        PreviousMovements = 0;
        NewMovements = 0;
        if (!samples.Visible)
        {
          samples.Visible = true;
          this.Size = this.OriginalSize;
        }
        else
        {
          samples.Visible = false;
          this.AutoSizeMode = AutoSizeMode.GrowOnly;
          this.Size = this.HiddenSize;
        }
      }

      /*
         if (PreviousMovements ==1 && NewMovements ==0)
         {
         }
        */
    }

    private void TLP_ControlRemoved(object sender, ControlEventArgs e)
    {
      this.Dispose();
    }
  }
}